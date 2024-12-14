using System.Text.Json;
using ACore.Attributes;
using ACore.Configuration;
using ACore.Extensions;
using ACore.Server.Services;
using ACore.Server.Storages.Contexts.EF.Helpers;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Definitions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Guid = System.Guid;

namespace ACore.Server.Storages.Contexts.EF;

public abstract partial class DbContextBase(DbContextOptions options, IACoreServerApp iaCoreServerApp, ILogger<DbContextBase> logger) : DbContext(options), IRepository
{
  private readonly DbContextOptions _options = options;
  protected readonly ILogger<DbContextBase> Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
  private readonly Dictionary<string, object> _registeredDbSets = [];

  protected abstract string ModuleName { get; }
  protected abstract DbScriptBase UpdateScripts { get; }
  protected abstract EFStorageDefinition EFStorageDefinition { get; }

  public StorageDefinition StorageDefinition => EFStorageDefinition;
  public RepositoryInfo RepositoryInfo => new(ModuleName, StorageDefinition);

  
  protected internal async Task<RepositoryOperationResult> Save<TEntity, TPK>(TEntity newData, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
  {
    ArgumentNullException.ThrowIfNull(newData);

    TEntity existsEntity;

    var id = newData.Id;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var databaseOperationEventHelper = new EntityEventHelper<TEntity, TPK>(iaCoreServerApp, Model, EFStorageDefinition, newData);
    await databaseOperationEventHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    var isNew = EFStorageDefinition.IsNew(id);
  
    var hashIsRequired = newData.GetType().IsSumHashAllowed();
    var saltForHash = string.Empty;
    if (hashIsRequired)
    {
      // Gets salt from global app settings.
      saltForHash = iaCoreServerApp.Options.ACoreOptions.SaltForHash;  //(await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt))).ResultValue ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");
      if (string.IsNullOrEmpty(saltForHash))
        Logger.LogWarning($"Please configure salt for hash. Check application settings and paste hash string to section '{nameof(ACoreOptions)}.{nameof(ACoreOptions.SaltForHash)}'");
    }

    if (!isNew)
    {
      var existsEntityNullable = await GetEntityById<TEntity, TPK>(id);
      if (existsEntityNullable == null)
        return RepositoryOperationResult.ErrorEntityNotExists(typeof(TEntity).Name, id.ToString() ?? string.Empty);
      
      existsEntity = existsEntityNullable;
      
      //check db entity concurrency with hash
      if (hashIsRequired)
      {
        if (hashToCheck == null)
          throw new ArgumentNullException($"For update entity '{typeof(TEntity).Name}:{id}' is required a checkSum hash.");
        
        //Check consistency of entity.
        if (hashToCheck != existsEntity.GetSumHash(saltForHash))
          return RepositoryOperationResult.ErrorConcurrency(typeof(TEntity).Name, id.ToString() ?? string.Empty);
        
        // Item has not been modified, save doesn't required.
        if (newData.GetSumHash(saltForHash) == hashToCheck)
          return RepositoryOperationResult.Success(RepositoryOperationTypeEnum.UnModified, hashToCheck);
      }

      databaseOperationEventHelper.UpdateEntityAction(existsEntity);
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
        throw new Exception($"Save entity '{existsEntity.GetType().ACoreTypeName()}' failed is rollback: Data {JsonSerializer.Serialize(existsEntity)}", ex);
      }
    }
    else
      await SaveInternal();

    return RepositoryOperationResult.Success(
      isNew ? RepositoryOperationTypeEnum.Added : RepositoryOperationTypeEnum.Modified, 
      hashIsRequired ? existsEntity.GetSumHash(saltForHash): null);

    async Task SaveInternal()
    {
      await SaveChangesAsync();
      if (isNew) databaseOperationEventHelper.AddEntityAction(existsEntity);

      if (databaseOperationEventHelper.EntityEventOperationItem != null && !_isDatabaseInit)
        await iaCoreServerApp.Mediator.Publish(new EntityEventNotification(databaseOperationEventHelper.EntityEventOperationItem));
    }
  }

  protected internal async Task<RepositoryOperationResult> Delete<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    var entityToDelete = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var saveInfoHelper = new EntityEventHelper<TEntity, TPK>(iaCoreServerApp, Model, EFStorageDefinition, entityToDelete);
    await saveInfoHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(entityToDelete);

    await SaveChangesAsync();
    saveInfoHelper.DeleteEntityAction();
    if (saveInfoHelper.EntityEventOperationItem != null)
      await iaCoreServerApp.Mediator.Publish(new EntityEventNotification(saveInfoHelper.EntityEventOperationItem));

    return RepositoryOperationResult.Success(RepositoryOperationTypeEnum.Deleted);
  }

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new ArgumentException($"{nameof(dbSet)} is null.");

    _registeredDbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  private static string GetEntityTypeName<T>()
    => typeof(T).FullName ?? throw new Exception($"{nameof(Type.FullName)} cannot be retrieved.");

  protected DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_registeredDbSets.TryGetValue(entityName, out var dbSet))
      return dbSet as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"No registered {nameof(DbSet<T>)} has not been found. Please call the function {nameof(RegisterDbSet)} in ctor.");
  }
  
  #region id

#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
  protected async Task<TEntity?> GetEntityById<TEntity, TPK>(TPK id)
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
#pragma warning restore CS8605 // Unboxing a possibly null value.

  #endregion
}