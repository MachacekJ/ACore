using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations;
using ACore.Tests.Server.TestImplementations.Configuration;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.AuditModule;

public class AuditModuleTestsBase(StorageTypeEnum st) : StorageTestsBase(st)
{
  private readonly IAuditUserProvider _userProvider = TestAuditUserProvider.CreateDefaultUser();

  protected override void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    base.SetupACoreServer(builder);
    builder.AddAuditModule(a => { a.UserProvider(_userProvider); });
  }

  protected override void SetupACoreTest(ACoreTestOptionsBuilder builder)
  {
    base.SetupACoreTest(builder);
    builder.AddTestModule();
  }
}