using System.Reflection;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Mongo;

/// <summary>
/// Two kinds of table primary key are supported.
/// ObjectId for mongoDb is tested in integration tests. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditMongoPKTests : AuditMongoTestsBase
{
  [Fact]
  public async Task ObjectIdPKNotImplTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.ObjectIdPK(Mediator, GetTableName, GetColumnName); });
  }
}