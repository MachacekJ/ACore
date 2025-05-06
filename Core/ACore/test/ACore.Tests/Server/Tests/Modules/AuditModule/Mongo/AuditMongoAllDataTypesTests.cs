using System.Reflection;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using MongoDB.Bson;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Mongo;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditMongoAllDataTypesTests : AuditMongoTestsBase
{
  [Fact]
  public async Task AllDataTypesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    MongoConventions.RegisterConventions();
    await RunTestAsync(method, async () => await AuditAllDataTypesTestHelper.AllDataTypes<ObjectId>(Mediator, LogInMemorySink, true, GetTableName, GetColumnName));
  }
}
