using System.Linq.Expressions;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG;

public static class DefaultNames
{
  public static Dictionary<string, EFInternalName> ObjectNameMapping => new()
  {
    { nameof(Fake1NoAuditEntity), new EFInternalName("fake1_no_audit", TestNoAuditEntityColumnNames) },
    { nameof(Fake1AuditEntity), new EFInternalName("fake1_audit", TestAuditEntityColumnNames) },
    { nameof(Fake1ValueTypeEntity), new EFInternalName("fake1_value_type", TestValueTypeEntityColumnNames) },
    { nameof(Fake1PKGuidEntity), new EFInternalName("fake1_pk_guid", TestPKGuidEntityColumnNames) },
    { nameof(Fake1PKStringEntity), new EFInternalName("fake1_pk_string", TestPKStringEntityColumnNames) },
    { nameof(Fake1PKLongEntity), new EFInternalName("fake1_pk_long", TestPKLongEntityColumnNames) }
  };

  private static Dictionary<Expression<Func<Fake1NoAuditEntity, object>>, string> TestNoAuditEntityColumnNames => new()
  {
    { e => e.Id, "fake1_no_audit_id" },
    { e => e.Name, "name" },
    { e => e.Created, "created" }
  };

  private static Dictionary<Expression<Func<Fake1AuditEntity, object>>, string> TestAuditEntityColumnNames => new()
  {
    { e => e.Id, "fake1_audit_id" },
    { e => e.Name, "name" },
    { e => e.NullValue, "null_value" },
    { e => e.NullValue2, "null_value2" },
    { e => e.NullValue3, "null_value3" },
    { e => e.NotAuditableColumn, "not_auditable_column" },
    { e => e.Created, "created" }
  };
  
  private static Dictionary<Expression<Func<Fake1ValueTypeEntity, object>>, string> TestValueTypeEntityColumnNames => new()
  {
    { e => e.Id, "fake1_value_type_id" },
    { e => e.IntNotNull, "int_not_null" },
    { e => e.IntNull, "int_null" },
    { e => e.BigIntNotNull, "big_int_not_null" },
    { e => e.BigIntNull, "big_int_null" },
    { e => e.Bit2, "bit2" },
    { e => e.Char2, "char2" },
    { e => e.Date2, "date2" },
    { e => e.DateTime2, "datetime2" },
    { e => e.Decimal2, "decimal2" },
    { e => e.NChar2, "nchar2" },
    { e => e.NVarChar2, "nvarchar2" },
    { e => e.SmallDateTime2, "smalldatetime2" },
    { e => e.SmallInt2, "smallint2" },
    { e => e.TinyInt2, "tinyint2" },
    { e => e.Guid2, "guid2" },
    { e => e.VarBinary2, "varbinary2" },
    { e => e.VarChar2, "varchar2" },
    { e => e.TimeSpan2, "timespan2" },
  };

  private static Dictionary<Expression<Func<Fake1PKGuidEntity, object>>, string> TestPKGuidEntityColumnNames => new()
  {
    { e => e.Id, "fake1_pk_guid_id" },
    { e => e.Name, "name" }
  };
  
  private static Dictionary<Expression<Func<Fake1PKStringEntity, object>>, string> TestPKStringEntityColumnNames => new()
  {
    { e => e.Id, "fake1_pk_string_id" },
    { e => e.Name, "name" }
  };
  
  private static Dictionary<Expression<Func<Fake1PKLongEntity, object>>, string> TestPKLongEntityColumnNames => new()
  {
    { e => e.Id, "fake1_pk_long_id" },
    { e => e.Name, "name" }
  };
}