using System.Reflection;
using ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditMemoryDbAllDataTypesTests : AuditMemoryDbTestsBase
{
  [Fact]
  public async Task AllDataTypesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => await AuditAllDataTypesTestHelper.AllDataTypes<int>(Mediator, LogInMemorySink, false, GetTableName, GetColumnName));
  }
}
