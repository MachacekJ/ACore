using System.Reflection;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using MongoDB.Bson;
using Xunit;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Mongo;

/// <summary>
/// Test audit items when entity class contains audit attributes.
/// </summary>
public class AuditMongoCRUDTests : AuditMongoTestsBase
{
  [Fact]
  public async Task NoAuditTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.NoAuditAsyncTest<ObjectId>(Mediator, GetTableName); });
  }
  
  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.AddItemAsyncTest<ObjectId>(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.UpdateItemAsyncTest<ObjectId>(Mediator, GetTableName, GetColumnName); });
  }
  
  [Fact]
  public async Task UpdateItemWithoutChangesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.UpdateItemWithoutChangesAsyncTest<ObjectId>(Mediator, GetTableName, GetColumnName); });
  }
  
  
  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.DeleteItemTest<ObjectId>(Mediator, GetTableName, GetColumnName); });
  }
}