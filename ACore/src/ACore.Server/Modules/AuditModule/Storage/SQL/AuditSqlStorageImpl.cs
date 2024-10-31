using System.Collections.Immutable;
using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Extensions;
using ACore.Server.Modules.AuditModule.Storage.Helpers;
using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Models.EntityEvent;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL;

internal abstract class AuditSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  private readonly IMediator _mediator = mediator;

  internal static CacheKey AuditColumnCacheKey(int tableId) => CacheKey.Create(CacheMainCategories.Entity, new CacheCategory(nameof(AuditColumnEntity)), tableId.ToString());
  internal static CacheKey AuditUserCacheKey(string userId) => CacheKey.Create(CacheMainCategories.Entity, new CacheCategory(nameof(AuditUserEntity)), userId);
  internal static CacheKey AuditTableCacheKey(string tableName, string? schema, int version) => CacheKey.Create(CacheMainCategories.Entity, new CacheCategory(nameof(AuditTableEntity)), $"{tableName}-{schema ?? string.Empty}-{version}");

  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditEntity> Audits { get; set; }
  internal DbSet<AuditColumnEntity> AuditColumns { get; set; }
  public DbSet<AuditUserEntity> AuditUsers { get; set; }
  public DbSet<AuditTableEntity> AuditTables { get; set; }
  public DbSet<AuditValueEntity> AuditValues { get; set; }

  public async Task<AuditInfoItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null)
  {
    AuditValueEntity[] auditValueEntities;
    if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
      auditValueEntities = await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValue == Convert.ToInt64(pkValue)).ToArrayAsync();
    else
      auditValueEntities = await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValueString == Convert.ToString(pkValue)).ToArrayAsync();

    var res = new List<AuditInfoItem>();
    foreach (var grItem in auditValueEntities.GroupBy(e =>
               new
               {
                 tableName = e.Audit.AuditTable.TableName,
                 schemaName = e.Audit.AuditTable.SchemaName,
                 version = e.Audit.AuditTable.Version,
                 pk = e.Audit.PKValue,
                 pkString = e.Audit.PKValueString,
                 entityState = e.Audit.EntityState,
                 user = e.Audit.User.UserId,
                 created = e.Audit.DateTime
               }))
    {
      var primaryKeyValue = (grItem.Key.pk == null) ? grItem.Key.pkString : grItem.Key.pk as object;
      if (primaryKeyValue == null)
        throw new Exception("Primary key is null");

      var auditEntryItem = new AuditInfoItem(grItem.Key.tableName, grItem.Key.schemaName, grItem.Key.version, primaryKeyValue, grItem.Key.entityState, grItem.Key.user)
      {
        Created = grItem.Key.created
      };

      foreach (var col in grItem.ToArray())
      {
        auditEntryItem.AddColumnEntry(new AuditInfoColumnItem(
          col.AuditColumn.PropName, col.AuditColumn.ColumnName,
          col.AuditColumn.DataType, col.IsChanged,
          col.GetOldValueObject().ConvertObjectToDataType(col.AuditColumn.DataType),
          col.GetNewValueObject().ConvertObjectToDataType(col.AuditColumn.DataType)
        ));
      }

      res.Add(auditEntryItem);
    }

    return res.ToArray();
  }

  public async Task SaveAuditAsync(EntityEventItem entityEventItem)
  {
    if (entityEventItem.IsAuditable == false || !entityEventItem.ChangedColumns.Any())
      return;

    var auditTableId = await AuditTableId(entityEventItem.TableName, entityEventItem.SchemaName, entityEventItem.Version);
    var auditEntity = new AuditEntity
    {
      AuditTableId = auditTableId,
      PKValue = entityEventItem.PkValue,
      PKValueString = entityEventItem.PkValueString,
      AuditUserId = await GetAuditUserIdAsync(entityEventItem.UserId),
      DateTime = DateTime.UtcNow,
      EntityState = entityEventItem.EntityState,
      AuditValues = new ObservableCollectionListSource<AuditValueEntity>()
    };

    var auditableColumns = entityEventItem.ChangedColumns.Where(e => e.IsAuditable).ToImmutableArray();
    var auditColumnIds = await AuditColumnId(auditTableId, auditableColumns);
    foreach (var value in auditableColumns)
    {
      auditEntity.AuditValues.Add(value.ToAuditSqlValue(auditColumnIds));
    }

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  private async Task<int> GetAuditUserIdAsync(string userId)
  {
    var keyCache = AuditUserCacheKey(userId);

    var cacheValue = await _mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      Logger.LogDebug("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
      return ((AuditUserEntity)cacheValue.ResultValue.ObjectValue).Id;
    }

    var userEntity = await AuditUsers.FirstOrDefaultAsync(u => u.UserId == userId);
    if (userEntity == null)
    {
      userEntity = new AuditUserEntity
      {
        UserId = userId
      };
      await AuditUsers.AddAsync(userEntity);
      await SaveChangesAsync();
      Logger.LogDebug("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    }

    await _mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, userEntity));
    Logger.LogDebug("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    return userEntity.Id;
  }

  private async Task<int> AuditTableId(string tableName, string? tableSchema, int version)
  {
    var keyCache = AuditTableCacheKey(tableName, tableSchema, version);

    var cacheValue = await _mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      Logger.LogDebug("Value from cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(AuditTableId), keyCache, tableName, tableSchema);
      return ((AuditTableEntity)cacheValue.ResultValue.ObjectValue).Id;
    }

    var tableEntity = await AuditTables.FirstOrDefaultAsync(u => u.TableName == tableName && u.SchemaName == tableSchema);
    if (tableEntity == null)
    {
      tableEntity = new AuditTableEntity
      {
        TableName = tableName,
        SchemaName = tableSchema,
        Version = version
      };

      await AuditTables.AddAsync(tableEntity);
      await SaveChangesAsync();
      Logger.LogDebug("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(AuditTableId), keyCache, tableName, tableSchema);
    }

    await _mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, tableEntity));
    Logger.LogDebug("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(AuditTableId), keyCache, tableName, tableSchema);
    return tableEntity.Id;
  }

  private async Task<Dictionary<string, int>> AuditColumnId(int tableId, IEnumerable<EntityEventColumnItem> columns)
  {
    var keyCache = AuditColumnCacheKey(tableId);
    var res = new Dictionary<string, int>();

    #region Cache values

    var cacheValue = await _mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      Logger.LogDebug("Value from cache:{AuditColumnId}:{keyCache}", nameof(AuditColumnId), keyCache);
      res = (Dictionary<string, int>)cacheValue.ResultValue.ObjectValue;
    }

    // Check if I have all columns from cache.
    var missingColumnNamesInCache = new List<(string PropName, string ColName, string PropType)>();
    foreach (var propName in columns)
    {
      if (res.Any(e => e.Key == propName.PropName))
        continue;

      missingColumnNamesInCache.Add((propName.PropName, propName.ColumnName, propName.DataType));
      Logger.LogDebug("Missing value in cache:{AuditColumnId}:{keyCache}:{propName}",
        nameof(AuditColumnId), keyCache, propName.PropName);
    }

    if (missingColumnNamesInCache.Count == 0)
      return res; //All columns ids are saved in cache

    res.Clear();

    #endregion

    // I haven't a column in cache. Trying to get from DB.
    var missingPropNames = new List<(string PropName, string ColName, string DataType)>();
    var dbColumnEntitiesForTable = await AuditColumns.Where(a => a.AuditTableId == tableId).ToListAsync();

    // Refresh response according to db.
    if (dbColumnEntitiesForTable.Count > 0)
    {
      foreach (var auditColumnEntity in dbColumnEntitiesForTable)
      {
        res.Add(auditColumnEntity.PropName, auditColumnEntity.Id);
        Logger.LogDebug("Value added from DB to cache:{AuditColumnId}:{keyCache}:{propName}"
          , nameof(AuditColumnId), keyCache, auditColumnEntity.PropName);
      }

      foreach (var missingDbColumn in missingColumnNamesInCache)
      {
        // New audit column is appeared for existing table, this column is not saved in db yet.
        if (dbColumnEntitiesForTable.SingleOrDefault(a => a.PropName == missingDbColumn.PropName) == null)
          missingPropNames.Add(missingDbColumn);
      }
    }
    else
    {
      // New audit table is appeared.
      missingPropNames.AddRange(missingColumnNamesInCache);
    }

    if (missingPropNames.Count > 0)
    {
      // I haven't a column in db. The columns must be saved in DB.
      var newDbColumnEntities = new List<AuditColumnEntity>();
      foreach (var propName in missingPropNames)
      {
        var columnEntity = new AuditColumnEntity
        {
          PropName = propName.PropName,
          ColumnName = propName.ColName,
          DataType = propName.DataType,
          AuditTableId = tableId
        };
        newDbColumnEntities.Add(columnEntity);
        await AuditColumns.AddAsync(columnEntity);
        Logger.LogDebug("New db value created:{AuditColumnId}:{keyCache}:{propName}",
          nameof(AuditColumnId), keyCache, propName.PropName);
      }

      await SaveChangesAsync();

      // I must add columns id to response.
      foreach (var savedColumnEntity in newDbColumnEntities)
      {
        res.Add(savedColumnEntity.PropName, savedColumnEntity.Id);
      }
    }

    await _mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, res));
    Logger.LogDebug("Value saved to cache:{AuditColumnId}:{keyCache}", nameof(AuditColumnId), keyCache);

    return res;
  }

  private IQueryable<AuditValueEntity> SelectVwAudits(string tableName, string? schemaName)
  {
    return AuditValues
      .Include(a => a.Audit)
      .Include(a => a.AuditColumn)
      .Include(a => a.Audit.AuditTable)
      .Include(a => a.Audit.User).Where(i => i.Audit.AuditTable.TableName == tableName && i.Audit.AuditTable.SchemaName == schemaName).Select(a => a);
  }
}