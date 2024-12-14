using ACore.Repository.Definitions.Models;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Tests.Server.FakeApp.Configuration;
using ACore.Tests.Server.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Mongo;

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);

public class AuditMongoTestsBase() : FakeAppTestsBase([RepositoryTypeEnum.Mongo])
{
  protected override void SetupBuilder(FakeAppOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(s=>s.DefaultRepositoryType(RepositoryTypeEnum.Mongo));
    builder.AddAuditModule();
    builder.AddTestModule();
    base.SetupBuilder(builder);
  }
  protected string GetTableName(Type entityName)
  {
    return entityName.GetCollectionName();
  }

  protected string GetColumnName(Type entityName, string propertyName)
  {
    return entityName.GetMongoEntityName(propertyName);
  }

 
}