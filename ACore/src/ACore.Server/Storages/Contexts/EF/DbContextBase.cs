using System.Data;
using ACore.Configuration;
using ACore.Extensions;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.Contexts.EF.Helpers;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Definitions;
using ACore.Server.Storages.Definitions.EF;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace ACore.Server.Storages.Contexts.EF;

public abstract partial class DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger) : DbContext(options), IStorage
{
  private readonly DbContextOptions _options = options;
  protected readonly ILogger<DbContextBase> Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
  private readonly Dictionary<string, object> _registeredDbSets = [];

  protected abstract DbScriptBase UpdateScripts { get; }

  public StorageDefinition StorageDefinition => EFStorageDefinition;

  protected abstract EFStorageDefinition EFStorageDefinition { get; }

  protected abstract string ModuleName { get; }

  protected internal async Task Save<TEntity, TPK>(TEntity newData, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
  {
    ArgumentNullException.ThrowIfNull(newData);

    TEntity existsEntity;

    var id = newData.Id;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var saveInfoHelper = new SaveInfoHelper<TEntity, TPK>(mediator, Model, EFStorageDefinition, newData);
    await saveInfoHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    var isNew = EFStorageDefinition.IsNew(id);

    if (!isNew)
    {
      existsEntity = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
      //check db entity concurrency with hash
      if (newData.GetType().IsHashCheck())
      {
        if (hashToCheck == null)
          throw new ArgumentNullException($"For update entity '{typeof(TEntity).Name}:{id}' is required a hash.");

        // Gets salt from global app settings.
        var saltForHash = (await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt))).ResultValue ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");

        if (string.IsNullOrEmpty(saltForHash))
          Logger.LogWarning($"Please configure salt for hash. Check application settings and paste hash string to section '{nameof(ACoreOptions)}.{nameof(ACoreOptions.SaltForHash)}'");

        //Check consistency of entity.
        if (hashToCheck != existsEntity.HashObject(saltForHash))
          throw new DBConcurrencyException($"Entity '{typeof(TEntity).Name}' with id '{id.ToString()}' has been changed.");
      }

      saveInfoHelper.UpdateDbAction(existsEntity);
      newData.Adapt(existsEntity);
    }
    else
    {
      existsEntity = newData;
      existsEntity.Id = EFStorageDefinition.NewId<TEntity, TPK>(dbSet);
      await dbSet.AddAsync(existsEntity);
    }

    if (EFStorageDefinition.IsTransactionEnabled)
    {
      await using var transaction = await Database.BeginTransactionAsync();
      try
      {
        await SaveInternal();
        await transaction.CommitAsync();
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        throw new Exception($"Save entity '{existsEntity.GetType().ACoreTypeName()}' failed is rollback: Data {JsonConvert.SerializeObject(existsEntity)}", ex);
      }
    }
    else
      await SaveInternal();

    return;

    async Task SaveInternal()
    {
      await SaveChangesAsync();
      if (isNew) saveInfoHelper.InsertDbAction(existsEntity);

      if (saveInfoHelper.SaveInfoItem != null && !_isDatabaseInit)
        await mediator.Publish(new EntitySaveNotification(saveInfoHelper.SaveInfoItem));
    }
  }

  protected async Task Delete<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    var entityToDelete = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var saveInfoHelper = new SaveInfoHelper<TEntity, TPK>(mediator, Model, EFStorageDefinition, entityToDelete);
    await saveInfoHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(entityToDelete);

    await SaveChangesAsync();
    saveInfoHelper.DeleteDbAction();
    if (saveInfoHelper.SaveInfoItem != null)
      await mediator.Publish(new EntitySaveNotification(saveInfoHelper.SaveInfoItem));
  }

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new ArgumentException($"{nameof(dbSet)} is null.");

    _registeredDbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  private static string GetEntityTypeName<T>()
    => typeof(T).FullName ?? throw new Exception($"{nameof(Type.FullName)} cannot be retrieved.");


  protected static void SetDatabaseNames<T>(Dictionary<string, EFNameDefinition> objectNameMapping, ModelBuilder modelBuilder) where T : class
  {
    if (objectNameMapping.TryGetValue(typeof(T).Name, out var auditColumnEntityObjectNames))
    {
      modelBuilder.Entity<T>().ToTable(auditColumnEntityObjectNames.TableName);
      foreach (var expression in auditColumnEntityObjectNames.GetColumns<T>())
      {
        modelBuilder.Entity<T>().Property(expression.Key).HasColumnName(expression.Value);
      }
    }
    else
    {
      throw new Exception($"Missing database name definition for entity: {typeof(T).Name}");
    }
  }

  protected DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_registeredDbSets.TryGetValue(entityName, out var aa))
      return aa as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"No registered {nameof(DbSet<T>)} has not been found. Please call the function {nameof(RegisterDbSet)} in ctor.");
  }
  
  #region id

#pragma warning disable CS8602 // Dereference of a possibly null reference.
  private async Task<TEntity?> GetEntityById<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    var remap = GetDbSet<TEntity>();
    
    if (typeof(PKEntity<int>).IsAssignableFrom(typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKEntity<int>).Id == Convert.ToInt32(id));
    
    if (typeof(PKEntity<long>).IsAssignableFrom(typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKEntity<long>).Id == Convert.ToInt64(id));
    
    if (typeof(PKEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKEntity<Guid>).Id == (Guid)Convert.ChangeType(id, typeof(Guid)));
    
    if (typeof(PKEntity<string>).IsAssignableFrom(typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKEntity<string>).Id == Convert.ToString(id));
    
    if (typeof(PKEntity<ObjectId>).IsAssignableFrom(typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKEntity<ObjectId>).Id == (ObjectId)Convert.ChangeType(id, typeof(ObjectId)));

    throw new Exception($"Unsupported type of primary key for entity '{typeof(TEntity).Name}.'");
  }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

  #endregion
}