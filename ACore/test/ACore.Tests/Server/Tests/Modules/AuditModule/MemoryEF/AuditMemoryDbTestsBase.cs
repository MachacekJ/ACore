using ACore.Repository.Definitions.Models;
using ACore.Tests.Server.FakeApp.Configuration;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;

public class AuditMemoryDbTestsBase() : FakeAppTestsBase([RepositoryTypeEnum.MemoryEF])
{
  protected override void SetupBuilder(FakeAppOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(s=>s.DefaultRepositoryType(RepositoryTypeEnum.MemoryEF));
    builder.AddAuditModule();
    builder.AddTestModule();
    base.SetupBuilder(builder);
  }
  protected string GetTableName(Type entityName)
  {
    return entityName.Name;
  }

  protected string GetColumnName(Type entityName, string propertyName)
  {
    return propertyName;
  }
}