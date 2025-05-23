﻿using System.Linq.Expressions;
using ACore.Server.Modules.AuditModule.Repositories.EF.Models;
using ACore.Server.Repository.Contexts.EF.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Server.Modules.AuditModule.Repositories.EF.PG;

internal static class DefaultNames
{
  public static Dictionary<string, EFInternalName> ObjectNameMapping => new()
  {
    { nameof(AuditColumnEntity), new EFInternalName("audit_column", AuditColumnEntityColumnNames) },
    { nameof(AuditEntity), new EFInternalName("audit", AuditEntityColumnNames) },
    { nameof(AuditTableEntity), new EFInternalName("audit_table", AuditTableEntityColumnNames) },
    { nameof(AuditUserEntity), new EFInternalName("audit_user", AuditUserEntityColumnNames) },
    { nameof(AuditValueEntity), new EFInternalName("audit_value", AuditValueColumnNames) },
  };

  private static Dictionary<Expression<Func<AuditColumnEntity, object>>, string> AuditColumnEntityColumnNames => new()
  {
    { e => e.Id, "audit_column_id" },
    { e => e.AuditTableId, "audit_table_id" },
    { e => e.PropName, "prop_name" },
    { e => e.ColumnName, "column_name" },
    { e => e.DataType, "data_type" }
  };

  private static Dictionary<Expression<Func<AuditEntity, object>>, string> AuditEntityColumnNames => new()
  {
    { e => e.Id, "audit_id" },
    { e => e.AuditTableId, "audit_table_id" },
    { e => e.PKValue, "pk_value" },
    { e => e.PKValueString, "pk_value_string" },
    { e => e.AuditUserId, "audit_user_id" },
    { e => e.DateTime, "date_time" },
    { e => e.EntityState, "entity_state" }
  };

  private static Dictionary<Expression<Func<AuditTableEntity, object>>, string> AuditTableEntityColumnNames => new()
  {
    { e => e.Id, "audit_table_id" },
    { e => e.TableName, "table_name" },
    { e => e.SchemaName, "schema_name" },
    { e => e.Version, "version" }
  };

  private static Dictionary<Expression<Func<AuditUserEntity, object>>, string> AuditUserEntityColumnNames => new()
  {
    { e => e.Id, "audit_user_id" },
    { e => e.UserId, "user_id" }
  };

  private static Dictionary<Expression<Func<AuditValueEntity, object>>, string> AuditValueColumnNames => new()
  {
    { e => e.Id, "audit_value_id" },
    { e => e.AuditId, "audit_id" },
    { e => e.IsChanged, "is_changed" },
    { e => e.AuditColumnId, "audit_column_id" },
    { e => e.OldValueString, "old_value_string" },
    { e => e.NewValueString, "new_value_string" },
    { e => e.OldValueInt, "old_value_int" },
    { e => e.NewValueInt, "new_value_int" },
    { e => e.OldValueLong, "old_value_long" },
    { e => e.NewValueLong, "new_value_long" },
    { e => e.OldValueBool, "old_value_bool" },
    { e => e.NewValueBool, "new_value_bool" },
    { e => e.OldValueGuid, "old_value_guid" },
    { e => e.NewValueGuid, "new_value_guid" }
  };
}