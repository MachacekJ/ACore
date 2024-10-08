using System.Linq.Expressions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;

public static class DefaultNames
{
  public static Dictionary<string, EFNameDefinition> ObjectNameMapping => new()
  {
    { nameof(Models.TestAuditEntity), new EFNameDefinition("test_audit", TestAuditEntity) },
    { nameof(TestValueTypeEntity), new EFNameDefinition("test_value_type", TestValueTypeEntityColumnNames) },
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
    // { e => e.IntNotNull, "int_not_null" },
    // { e => e.IntNull, "int_null" },
    // { e => e.BigIntNotNull, "big_int_not_null" },
    // { e => e.BigIntNull, "big_int_null" },
    // { e => e.Bit2, "bit2" },
    // { e => e.Char2, "char2" },
    // { e => e.Date2, "date2" },
    // { e => e.DateTime2, "datetime2" },
    // { e => e.Decimal2, "decimal2" },
    // { e => e.NChar2, "nchar2" },
    // { e => e.NVarChar2, "nvarchar2" },
    // { e => e.SmallDateTime2, "smalldatetime2" },
    // { e => e.SmallInt2, "smallint2" },
    // { e => e.TinyInt2, "tinyint2" },
    // { e => e.Guid2, "guid2" },
    // { e => e.VarBinary2, "varbinary2" },
    // { e => e.VarChar2, "varchar2" },
  };
}