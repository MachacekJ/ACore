﻿using System.Reflection;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;

/// <summary>
/// Two kinds of table primary key are supported.
/// ObjectId for mongoDb is tested in integration tests. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditMemoryDbPKTests : AuditMemoryDbTestsBase
{
  [Fact]
  public async Task IntPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.IntPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task LongPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.LongPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task GuidPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.GuidPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task StringPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.StringPK(Mediator, GetTableName, GetColumnName); });
  }
  
  [Fact]
  public async Task ObjectIdPKNotImplTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Mongo entity with pk is not implemented in memory storage.
      await AuditPKTestHelper.ObjectIdPKNotImplemented(Mediator);
    });
  }
}