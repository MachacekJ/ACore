using ACore.Extensions;
using ACore.Models.Cache;
using ACore.Server.Modules.AuditModule.Repositories.SQL;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Models;
using ACore.Server.Services;
using ACore.Server.Services.ServerCache;
using ACore.Server.Storages.Models.EntityEvent;
using ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL;

public class AuditSqlStorageImplCacheTests
{
  private const string FakeTableName = "fakeTable";
  private const string FakeUserName = "fakeUser";
  private static readonly CacheKey TableCacheKey = AuditSqlRepositoryImpl.AuditTableCacheKey(FakeTableName, null, 1);
  private static readonly CacheKey UserCacheKey = AuditSqlRepositoryImpl.AuditUserCacheKey(FakeUserName);

  [Fact]
  public async Task NoCacheValuesTest()
  {
    // Arrange.
    var cacheQueryCalls = new List<string>();
    var cacheSaveCalls = new List<string>();
    var saveInfoItem = CreateSaveInfoItem();

    var appMocked = CreateApp(
      CreateServerCacheMocked(cacheQueryCalls, cacheSaveCalls, null, null, null)
    );

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(appMocked);

    // Act.
    await auditSqlStorageImplAsSut.SaveAuditAsync(saveInfoItem);

    // Assert
    EntityAsserts(auditSqlStorageImplAsSut, saveInfoItem);
    TableCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    UserCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    ColumnCacheCallsAssert(cacheQueryCalls, cacheSaveCalls, auditSqlStorageImplAsSut.AuditTables.Single(e => e.TableName == FakeTableName).Id);
  }

  [Fact]
  public async Task ExistingTableCacheValuesTest()
  {
    // Arrange
    var cacheQueryCalls = new List<string>();
    var cacheSaveCalls = new List<string>();
    var saveInfoItem = CreateSaveInfoItem();
    var auditTableEntity = new AuditTableEntity
    {
      SchemaName = saveInfoItem.SchemaName,
      Version = saveInfoItem.Version,
      TableName = saveInfoItem.TableName,
    };

    var appMocked = CreateApp(
      CreateServerCacheMocked(cacheQueryCalls, cacheSaveCalls,
        mem => mem.ToString() == TableCacheKey.ToString() ? auditTableEntity : null,
        null, null)
    );

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(appMocked, (db) =>
    {
      db.AuditTables.Add(auditTableEntity);
      db.SaveChanges();
    });

    // Act.
    await auditSqlStorageImplAsSut.SaveAuditAsync(saveInfoItem);

    // Assert
    EntityAsserts(auditSqlStorageImplAsSut, saveInfoItem);
    UserCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    ColumnCacheCallsAssert(cacheQueryCalls, cacheSaveCalls, auditSqlStorageImplAsSut.AuditTables.Single(e => e.TableName == FakeTableName).Id);
    cacheQueryCalls.Where(e => e == TableCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == TableCacheKey.ToString()).ToList().Should().HaveCount(0);
  }

  [Fact]
  public async Task ExistingUserCacheValuesTest()
  {
    // Arrange
    var cacheQueryCalls = new List<string>();
    var cacheSaveCalls = new List<string>();
    var saveInfoItem = CreateSaveInfoItem();
    var auditUserEntity = new AuditUserEntity
    {
      UserId = saveInfoItem.UserId
    };

    var appMocked = CreateApp(
      CreateServerCacheMocked(cacheQueryCalls, cacheSaveCalls,
        null,
        mem => mem.ToString() == UserCacheKey.ToString() ? auditUserEntity : null,
        null)
    );

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(appMocked, (db) =>
    {
      db.AuditUsers.Add(auditUserEntity);
      db.SaveChanges();
    });

    // Act.
    await auditSqlStorageImplAsSut.SaveAuditAsync(saveInfoItem);

    // Assert
    EntityAsserts(auditSqlStorageImplAsSut, saveInfoItem);
    TableCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    ColumnCacheCallsAssert(cacheQueryCalls, cacheSaveCalls, auditSqlStorageImplAsSut.AuditTables.Single(e => e.TableName == FakeTableName).Id);

    cacheQueryCalls.Where(e => e == UserCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == UserCacheKey.ToString()).ToList().Should().HaveCount(0);
  }

  [Fact]
  public async Task ExistingColumnCacheValuesTest()
  {
    // Arrange
    var idTable = 1;
    var columnCacheKey = AuditSqlRepositoryImpl.AuditColumnCacheKey(idTable);
    var cacheQueryCalls = new List<string>();
    var cacheSaveCalls = new List<string>();
    var saveInfoItem = CreateSaveInfoItem();
    var auditColumnEntities = new List<AuditColumnEntity>
    {
      new()
      {
        AuditTableId = idTable,
        ColumnName = saveInfoItem.ChangedColumns[0].ColumnName,
        PropName = saveInfoItem.ChangedColumns[0].PropName,
        DataType = saveInfoItem.ChangedColumns[0].DataType,
      },
      new()
      {
        AuditTableId = idTable,
        ColumnName = saveInfoItem.ChangedColumns[1].ColumnName,
        PropName = saveInfoItem.ChangedColumns[1].PropName,
        DataType = saveInfoItem.ChangedColumns[1].DataType,
      }
    };
    var appMocked = CreateApp(
      CreateServerCacheMocked(cacheQueryCalls, cacheSaveCalls,
        null, null,
        mem => mem.ToString() == columnCacheKey.ToString() ? auditColumnEntities.ToDictionary(auditColumnEntity => auditColumnEntity.PropName, auditColumnEntity => auditColumnEntity.Id) : null
      )
    );
    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(appMocked, (db) =>
    {
      db.AuditColumns.AddRange(auditColumnEntities);
      db.SaveChanges();
    });

    // Act.
    await auditSqlStorageImplAsSut.SaveAuditAsync(saveInfoItem);

    // Assert
    EntityAsserts(auditSqlStorageImplAsSut, saveInfoItem);
    TableCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    UserCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);

    cacheQueryCalls.Where(e => e == columnCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == columnCacheKey.ToString()).ToList().Should().HaveCount(0);
  }

  [Fact]
  public async Task MissingColumnCacheValuesTest()
  {
    // Arrange
    var idTable = 1;
    var columnCacheKey = AuditSqlRepositoryImpl.AuditColumnCacheKey(idTable);
    var cacheQueryCalls = new List<string>();
    var cacheSaveCalls = new List<string>();
    var saveInfoItem = CreateSaveInfoItem();
    saveInfoItem.ChangedColumns.Add(new EntityEventColumnItem(true, "TestProp3", "TestColumn3", typeof(int).ACoreTypeName(), true, 1, 2));
    var auditColumnEntities = new List<AuditColumnEntity>
    {
      new()
      {
        AuditTableId = idTable,
        ColumnName = saveInfoItem.ChangedColumns[0].ColumnName,
        PropName = saveInfoItem.ChangedColumns[0].PropName,
        DataType = saveInfoItem.ChangedColumns[0].DataType,
      },
      new()
      {
        AuditTableId = idTable,
        ColumnName = saveInfoItem.ChangedColumns[1].ColumnName,
        PropName = saveInfoItem.ChangedColumns[1].PropName,
        DataType = saveInfoItem.ChangedColumns[1].DataType,
      }
    };
    var appMocked = CreateApp(
      CreateServerCacheMocked(cacheQueryCalls, cacheSaveCalls,
        null, null,
        mem => mem.ToString() == columnCacheKey.ToString() ? auditColumnEntities.ToDictionary(auditColumnEntity => auditColumnEntity.PropName, auditColumnEntity => auditColumnEntity.Id) : null
      )
    );

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(appMocked, (db) =>
    {
      db.AuditColumns.AddRange(auditColumnEntities);
      db.SaveChanges();
    });

    // Act.
    await auditSqlStorageImplAsSut.SaveAuditAsync(saveInfoItem);

    // Assert
    EntityAsserts(auditSqlStorageImplAsSut, saveInfoItem);
    TableCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    UserCacheCallsAssert(cacheQueryCalls, cacheSaveCalls);
    ColumnCacheCallsAssert(cacheQueryCalls, cacheSaveCalls, auditSqlStorageImplAsSut.AuditTables.Single(e => e.TableName == FakeTableName).Id);
  }

  private AuditSqlRepositoryImpl CreateAuditSqlStorageImplAsSut(IACoreServerApp mediator, Action<FakeAuditSqlRepositoryImpl>? seedData = null)
  {
    var dbContextOptions = new DbContextOptions<AuditSqlRepositoryImpl>();
    var loggerMocked = new MoqLogger<AuditSqlMemoryRepositoryImpl>().LoggerMocked;
    var res = new FakeAuditSqlRepositoryImpl(dbContextOptions, mediator, loggerMocked);
    seedData?.Invoke(res);
    return res;
  }

  private EntityEventItem CreateSaveInfoItem()
    => new(true, FakeTableName, null, 1, 1, EntityEventEnum.Added, FakeUserName)
    {
      ChangedColumns =
      [
        new EntityEventColumnItem(true, "TestProp1", "TestColumn1", typeof(int).ACoreTypeName(), true, 1, 2),
        new EntityEventColumnItem(true, "TestProp2", "TestColumn2", typeof(int).ACoreTypeName(), true, 1, 2),
      ]
    };

  private void SetupMemoryCacheSaveCommand(Mock<IServerCache> cache, List<string> cacheSaveCalls)
  {
    cache.Setup(i => i.Set(It.IsAny<CacheKey>(), It.IsAny<It.IsAnyType>()))
      .Callback<CacheKey, object>((key, value)
        => cacheSaveCalls.Add(key.ToString()));
  }

  private IACoreServerApp CreateApp(IServerCache cache)
  {
    var mediator = new Mock<IACoreServerApp>();
    mediator.Setup(i => i.ServerCache)
      .Returns(cache);
    return mediator.Object;
  }

  private IServerCache CreateServerCacheMocked(List<string> cacheQueryCalls, List<string> cacheSaveCalls,
    Func<CacheKey, AuditTableEntity?>? auditTableCacheFunc,
    Func<CacheKey, AuditUserEntity?>? auditUserCacheFunc,
    Func<CacheKey, Dictionary<string, int>?>? columnCacheFunc)
  {
    var cache = new Mock<IServerCache>();
    if (auditTableCacheFunc != null)
      SetupMemoryCacheQueryCommand(cache, cacheQueryCalls, auditTableCacheFunc);
    else
      SetupMemoryCacheQueryCommand<AuditTableEntity?>(cache, cacheQueryCalls, _ => null);

    if (auditUserCacheFunc != null)
      SetupMemoryCacheQueryCommand(cache, cacheQueryCalls, auditUserCacheFunc);
    else
      SetupMemoryCacheQueryCommand<AuditUserEntity?>(cache, cacheQueryCalls, _ => null);

    if (columnCacheFunc != null)
      SetupMemoryCacheQueryCommand(cache, cacheQueryCalls, columnCacheFunc);
    else
      SetupMemoryCacheQueryCommand<Dictionary<string, int>?>(cache, cacheQueryCalls, _ => null);

    SetupMemoryCacheSaveCommand(cache, cacheSaveCalls);

    return cache.Object;
  }

  private void SetupMemoryCacheQueryCommand<T>(Mock<IServerCache> cache, List<string> cacheQueryCalls, Func<CacheKey, T> customResultFunc)
  {
    cache.Setup(i => i.Get<T>(It.IsAny<CacheKey>()))
      .Returns<CacheKey>((key) =>
      {
        cacheQueryCalls.Add(key.ToString());
        var res = customResultFunc.Invoke(key);
        return Task.FromResult(res);
      });
  }

  private void EntityAsserts(AuditSqlRepositoryImpl auditSqlRepositoryImplAsSut, EntityEventItem entityEventItem)
  {
    auditSqlRepositoryImplAsSut.AuditTables.Count().Should().Be(1);
    auditSqlRepositoryImplAsSut.Audits.Count().Should().Be(1);
    auditSqlRepositoryImplAsSut.AuditUsers.Count().Should().Be(1);
    auditSqlRepositoryImplAsSut.AuditColumns.Count().Should().Be(entityEventItem.ChangedColumns.Count);
  }

  private void TableCacheCallsAssert(IEnumerable<string> cacheQueryCalls, IEnumerable<string> cacheSaveCalls)
  {
    cacheQueryCalls.Where(e => e == TableCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == TableCacheKey.ToString()).ToList().Should().HaveCount(1);
  }

  private void UserCacheCallsAssert(IEnumerable<string> cacheQueryCalls, IEnumerable<string> cacheSaveCalls)
  {
    cacheQueryCalls.Where(e => e == UserCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == UserCacheKey.ToString()).ToList().Should().HaveCount(1);
  }

  private void ColumnCacheCallsAssert(IEnumerable<string> cacheQueryCalls, IEnumerable<string> cacheSaveCalls, int idTable)
  {
    var columnCacheKey = AuditSqlRepositoryImpl.AuditColumnCacheKey(idTable);
    cacheQueryCalls.Where(e => e == columnCacheKey.ToString()).ToList().Should().HaveCount(1);
    cacheSaveCalls.Where(e => e == columnCacheKey.ToString()).ToList().Should().HaveCount(1);
  }
}