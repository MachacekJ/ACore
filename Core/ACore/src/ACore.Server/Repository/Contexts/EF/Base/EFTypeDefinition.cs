using ACore.Repository.Definitions;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Repository.Contexts.EF.Base;

public abstract class EFTypeDefinition : RepositoryDefinition
{
  /// <summary>
  /// For getting column name (in PG database) or property name in repository.
  /// The name is usually used in attribute for specific entity.  
  /// </summary>
  public abstract string DataAnnotationColumnNameKey { get; }

  /// <summary>
  /// For getting table name or collection name in repository.
  /// The name is usually used in attribute for specific entity.
  /// </summary>
  public abstract string DataAnnotationTableNameKey { get; }

  /// <summary>
  /// Use transaction for save to repository.
  /// </summary>
  public abstract bool IsTransactionEnabled { get; }

  /// <summary>
  /// Is database after first schema up. e.g. I cannot save audit information about change data in setting table, before audit database tables are created.  
  /// </summary>
  public abstract Task<bool> DatabaseHasFirstUpdate<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<EFContextBase> logger)
    where T : DbContext;

  /// <summary>
  /// For a relational database, use the value 0 if the primary key automatically generates the id after saving. 
  /// </summary>
  protected virtual int CreatePKInt<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>
    => PKIntEntity.NewId;

  /// <summary>
  /// For a relational database, use the value 0 if the primary key automatically generates the id after saving. 
  /// </summary>
  protected virtual long CreatePKLong<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>
    => PKLongEntity.NewId;

  protected virtual Guid CreatePKGuid<TEntity, TPK>()
    where TEntity : PKEntity<TPK>
    => PKGuidEntity.NewId;

  protected virtual string CreatePKString<TEntity, TPK>()
    where TEntity : PKEntity<TPK>
    => PKStringEntity.NewId;


  public TPK NewId<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>
  {
    return typeof(TPK) switch
    {
      { } entityType when entityType == typeof(int) => (TPK)Convert.ChangeType(CreatePKInt<TEntity, TPK>(dbSet), typeof(TPK)),
      { } entityType when entityType == typeof(long) => (TPK)Convert.ChangeType(CreatePKLong<TEntity, TPK>(dbSet), typeof(TPK)),
      { } entityType when entityType == typeof(string) => (TPK)Convert.ChangeType(CreatePKString<TEntity, TPK>(), typeof(TPK)),
      { } entityType when entityType == typeof(Guid) => (TPK)Convert.ChangeType(CreatePKGuid<TEntity, TPK>(), typeof(TPK)),
      _ => throw new Exception("Unknown primary data type {}")
    };
  }

  public bool IsNew<TPK>(TPK id)
  {
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    return typeof(TPK) switch
    {
      { } entityType when entityType == typeof(int) => (int)Convert.ChangeType(id, typeof(int)) == PKIntEntity.EmptyId,
      { } entityType when entityType == typeof(long) => (long)Convert.ChangeType(id, typeof(long)) == PKLongEntity.EmptyId,
      { } entityType when entityType == typeof(string) => (string)Convert.ChangeType(id, typeof(string)) == PKStringEntity.EmptyId,
      { } entityType when entityType == typeof(Guid) => (Guid)Convert.ChangeType(id, typeof(Guid)) == PKGuidEntity.EmptyId,
     // { } entityType when entityType == typeof(ObjectId) => (ObjectId)Convert.ChangeType(id, typeof(ObjectId)) == PKMongoEntity.EmptyId,
      _ => throw new Exception("Unknown primary data type {}")
    };
  }
  
  public string GetTableName(IEntityType dbEntityType)
  {
    var tableName = dbEntityType.GetTableName();
    if (string.IsNullOrEmpty(DataAnnotationTableNameKey))
      return tableName ?? throw new Exception($"Unknown db table name for entity class '{dbEntityType.Name}'");

    var anno = dbEntityType.GetAnnotation(DataAnnotationTableNameKey).Value?.ToString();
    if (anno != null)
      tableName = anno;

    return tableName ?? throw new Exception($"Unknown db table name for entity class '{dbEntityType.Name}'");
  }
}