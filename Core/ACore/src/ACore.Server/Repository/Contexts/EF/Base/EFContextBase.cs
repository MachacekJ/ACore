using ACore.Repository;
using ACore.Repository.Models;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Contexts.Helpers;
using ACore.Server.Repository.Contexts.Helpers.Models;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Guid = System.Guid;

namespace ACore.Server.Repository.Contexts.EF.Base;

public abstract partial class EFContextBase : DbContext, IRepository
{
  private readonly DbContextOptions _options;
  private readonly Dictionary<string, object> _registeredDbSets = [];
  private readonly IACoreServerCurrentScope _serverCurrentScope;
  private readonly ILogger<EFContextBase> _logger;
  private readonly DatabaseCRUDHelper _databaseCRUDHelper;

  protected ILogger<EFContextBase> Logger => _logger;

  protected EFContextBase(DbContextOptions options, IACoreServerCurrentScope serverCurrentScope, ILogger<EFContextBase> logger) : base(options)
  {
    _serverCurrentScope = serverCurrentScope;
    _logger = logger;
    _options = options;
    _databaseCRUDHelper = new DatabaseCRUDHelper(serverCurrentScope, _logger);
  }

  protected abstract string ModuleName { get; }
  protected abstract List<EFVersionScriptsBase> AllUpdateVersions { get; }
  protected abstract EFTypeDefinition EFTypeDefinition { get; }

  public RepositoryInfo RepositoryInfo => new(ModuleName, EFTypeDefinition.Type);

  protected internal async Task<RepositoryOperationResult> Save<TEntity, TPK>(TEntity newData, string? oldValueHash = null)
    where TEntity : PKEntity<TPK>
  {
    var helperAuditInfo = GetAuditDefinitions<TEntity, TPK>();
    var crud = GetCRUDDefinitions<TEntity, TPK>();
    var entityId = newData.Id;
    if (entityId == null)
      ArgumentNullException.ThrowIfNull(entityId);

    return await _databaseCRUDHelper.Save(newData, crud, helperAuditInfo,
      EFTypeDefinition.IsNew(entityId),
      _isDatabaseInFirstInit,
      oldValueHash
    );
  }

  protected internal async Task<RepositoryOperationResult> Delete<TEntity, TPK>(TPK id, string? oldValueHash = null)
    where TEntity : PKEntity<TPK>
  {
    var helperAuditInfo = GetAuditDefinitions<TEntity, TPK>();
    var crud = GetCRUDDefinitions<TEntity, TPK>();

    return await _databaseCRUDHelper.Delete(id, crud, helperAuditInfo, oldValueHash);
  }

  private DatabaseAuditDefinitions GetAuditDefinitions<TEntity, TPK>()
    where TEntity : PKEntity<TPK>
  {
    var dbEntityType = Model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var tableName = EFTypeDefinition.GetTableName(dbEntityType);

    return new DatabaseAuditDefinitions(
      tableName,
      dbEntityType.GetSchema(),
      false,
      p => GetColumnName<TEntity>(p, dbEntityType));
  }

  private DatabaseCRUDDefinitions<TEntity, TPK> GetCRUDDefinitions<TEntity, TPK>()
    where TEntity : PKEntity<TPK>
  {
    var dbSet = GetDbSet<TEntity>();
    return
      new(
        () => EFTypeDefinition.NewId<TEntity, TPK>(dbSet),
        async (id) => await GetEntityById<TEntity, TPK>(id),
        async (ent) => await dbSet.AddAsync(ent),
        _ => Task.CompletedTask,
        async id => dbSet.Remove(await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"Item {typeof(TEntity).Name}:{id} doesn't exist.")),
        async () => await SaveChangesAsync());
  }

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new ArgumentException($"{nameof(dbSet)} is null.");

    _registeredDbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  protected DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_registeredDbSets.TryGetValue(entityName, out var dbSet))
      return dbSet as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"No registered {nameof(DbSet<T>)} has not been found. Please call the function {nameof(RegisterDbSet)} in ctor.");
  }

  private (string Name, bool IsAuditable) GetColumnName<T>(string propName, IEntityType dbEntityType)
  {
    if (propName.StartsWith('.'))
      propName = propName.Substring(1);

    var isAuditable = typeof(T).IsPropertyAuditable(propName);

    var property = dbEntityType.GetProperties().SingleOrDefault(property => property.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
    if (property == null)
      throw new Exception($"Unknown property '{propName}' on type '{typeof(T).Name}'");

    var columnName = property.GetColumnName();
    if (string.IsNullOrEmpty(EFTypeDefinition.DataAnnotationColumnNameKey))
      return (columnName, isAuditable);

    var anno = property.GetAnnotations().FirstOrDefault(e => e.Name == EFTypeDefinition.DataAnnotationColumnNameKey);
    if (anno != null)
      columnName = anno.Value?.ToString() ?? throw new Exception($"Annotation '{EFTypeDefinition.DataAnnotationColumnNameKey}' has not been found for property '{propName}'");

    return (columnName, isAuditable);
  }

  private static string GetEntityTypeName<T>()
    => typeof(T).FullName ?? throw new Exception($"{nameof(Type.FullName)} cannot be retrieved.");
  
  #region id

#pragma warning disable CS8605 // Unboxing a possibly null value.
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
#pragma warning restore CS8605 // Unboxing a possibly null value.

  #endregion
}