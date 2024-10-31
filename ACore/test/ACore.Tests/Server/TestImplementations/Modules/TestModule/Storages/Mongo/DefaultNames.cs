using System.Linq.Expressions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;

public static class DefaultNames
{
  public static Dictionary<string, EFDbNames> ObjectNameMapping => new()
  {
    { nameof(Models.TestAuditEntity), new EFDbNames("test_audit", TestAuditEntity) },
    { nameof(TestValueTypeEntity), new EFDbNames("test_value_type", TestValueTypeEntityColumnNames) },
  };
  
  private static Dictionary<Expression<Func<TestAuditEntity, object>>, string> TestAuditEntity => new()
  {
    { e => e.Id, "_id" },
    { e => e.Name, "name" },
    { e => e.NotAuditableColumn, "notAuditableColumn" },
    { e => e.Created, "created" },
    { e => e.NullValue, "nullValue" },
    { e => e.NullValue2, "nullValue2" },
    { e => e.NullValue3, "nullValue3" },
  };
  
  
  private static Dictionary<Expression<Func<TestValueTypeEntity, object>>, string> TestValueTypeEntityColumnNames => new()
  {
    { e => e.Id, "_id" },
  };
}