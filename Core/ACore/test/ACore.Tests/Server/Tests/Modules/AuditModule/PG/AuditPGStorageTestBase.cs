using ACore.Repository.Definitions.Models;
using ACore.Tests.Server.FakeApp.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.PG;

public class AuditPGStorageTestBase() : FakeAppTestsBase([RepositoryTypeEnum.Postgres])
{
  protected override void SetupBuilder(FakeAppOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(s=>s.DefaultRepositoryType(RepositoryTypeEnum.Postgres));
    builder.AddAuditModule();
    builder.AddTestModule();
    base.SetupBuilder(builder);
  }

  protected string GetTableName(Type entityName)
  {
    return DefaultNames.ObjectNameMapping[entityName.Name].TableName;
  }

  protected string GetColumnName(Type entityName, string propertyName)
  {
    return DefaultNames.ObjectNameMapping[entityName.Name].ColumnNames?[propertyName] ?? throw new InvalidDataException("Define column names for PG.");
  }
}