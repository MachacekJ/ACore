using ACore.Server.Configuration;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations.Configuration;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.AuditModule;

public class AuditModuleTestsBase(StorageTypeEnum st) : StorageTestsBase(st)
{
  protected override void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    base.SetupACoreServer(builder);
    builder.AddAuditModule();
  }

  protected override void SetupACoreTest(ACoreTestOptionsBuilder builder)
  {
    base.SetupACoreTest(builder);
    builder.AddTestModule();
  }
}