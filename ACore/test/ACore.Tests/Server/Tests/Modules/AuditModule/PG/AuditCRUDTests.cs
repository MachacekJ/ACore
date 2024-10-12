﻿using System.Reflection;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using Xunit;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace ACore.Tests.Server.Tests.Modules.AuditModule.PG;

/// <summary>
/// Test audit items when entity class contains audit attributes.
/// </summary>
public class AuditModulePGStorageTestBaseTests : AuditModulePGStorageTestBase
{
  [Fact]
  public async Task NoAuditTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.NoAuditAsyncTest(Mediator, GetTableName); });
  }


  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.AddItemAsyncTest<int>(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.UpdateItemAsyncTest(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemWithoutChangesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.UpdateItemWithoutChangesAsyncTest(Mediator, GetTableName, GetColumnName); });
  }


  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDTestHelper.DeleteItemTest(Mediator, GetTableName, GetColumnName); });
  }
}