using ACore.Extensions;
using ACore.Models.Cache;
using ACore.Models.Result;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.AuditModule.Repositories.SQL;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Models;
using ACore.Server.Storages.Models.EntityEvent;
using ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using MediatR;
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
    var mediator = new Mock<IMediator>();
    SetupMemoryCacheQueryCommand(mediator, cacheQueryCalls);
    SetupMemoryCacheSaveCommand(mediator, cacheSaveCalls);
    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(mediator.Object);

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
    var mediator = new Mock<IMediator>();
    SetupMemoryCacheSaveCommand(mediator, cacheSaveCalls);
    SetupMemoryCacheQueryCommand(mediator, cacheQueryCalls, mem
      => Result.Success(mem?.Key.ToString() == TableCacheKey.ToString() ? new CacheValue(auditTableEntity) : new CacheValue(null)));
    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(mediator.Object, (db) =>
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
    var mediator = new Mock<IMediator>();
    SetupMemoryCacheSaveCommand(mediator, cacheSaveCalls);
    SetupMemoryCacheQueryCommand(mediator, cacheQueryCalls, mem
      => Result.Success(mem?.Key.ToString() == UserCacheKey.ToString() ? new CacheValue(auditUserEntity) : new CacheValue(null)));

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(mediator.Object, (db) =>
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
    var mediator = new Mock<IMediator>();
    SetupMemoryCacheSaveCommand(mediator, cacheSaveCalls);
    SetupMemoryCacheQueryCommand(mediator, cacheQueryCalls, mem
      =>
    {
      var dic = auditColumnEntities.ToDictionary(auditColumnEntity => auditColumnEntity.PropName, auditColumnEntity => auditColumnEntity.Id);
      return Result.Success(mem?.Key.ToString() == columnCacheKey.ToString() ? new CacheValue(dic) : new CacheValue(null));
    });

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(mediator.Object, (db) =>
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
    var mediator = new Mock<IMediator>();
    SetupMemoryCacheSaveCommand(mediator, cacheSaveCalls);
    SetupMemoryCacheQueryCommand(mediator, cacheQueryCalls, mem
      =>
    {
      var dic = auditColumnEntities.ToDictionary(auditColumnEntity => auditColumnEntity.PropName, auditColumnEntity => auditColumnEntity.Id);
      return Result.Success(mem?.Key.ToString() == columnCacheKey.ToString() ? new CacheValue(dic) : new CacheValue(null));
    });

    var auditSqlStorageImplAsSut = CreateAuditSqlStorageImplAsSut(mediator.Object, (db) =>
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

  private AuditSqlRepositoryImpl CreateAuditSqlStorageImplAsSut(IMediator mediator, Action<FakeAuditSqlRepositoryImpl>? seedData = null)
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

  private void SetupMemoryCacheSaveCommand(Mock<IMediator> mediator, List<string> cacheSaveCalls)
  {
    var result = Result.Success(new CacheValue(null));
    mediator
      .Setup(i => i.Send(It.IsAny<MemoryCacheModuleSaveCommand>(), It.IsAny<CancellationToken>()))
      .Callback<IRequest<Result>, CancellationToken>((q, _) =>
      {
        if (q is MemoryCacheModuleSaveCommand mem)
          cacheSaveCalls.Add(mem.Key.ToString());
      })
      .ReturnsAsync(() => result);
  }

  private void SetupMemoryCacheQueryCommand(Mock<IMediator> mediator, List<string> cacheQueryCalls, Func<MemoryCacheModuleGetQuery?, Result<CacheValue>>? customResultFunc = null)
  {
    var result = Result.Success(new CacheValue(null));
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    mediator
      .Setup(i => i.Send(It.IsAny<MemoryCacheModuleGetQuery>(), It.IsAny<CancellationToken>()))
      .Callback<IRequest<Result<CacheValue?>>, CancellationToken>((q, _) =>
      {
        MemoryCacheModuleGetQuery? mem2 = null;
        if (q is MemoryCacheModuleGetQuery mem)
        {
          mem2 = mem;
          cacheQueryCalls.Add(mem.Key.ToString());
        }

        if (customResultFunc != null)
          result = customResultFunc(mem2);
      })
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
      .ReturnsAsync(() => result);
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