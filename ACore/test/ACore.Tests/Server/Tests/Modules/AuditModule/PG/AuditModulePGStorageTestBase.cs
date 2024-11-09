using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.PG;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.PG;

public class AuditModulePGStorageTestBase() : AuditModuleTestsBase(StorageTypeEnum.Postgres)
{
  protected string GetTableName(string entityName)
  {
    return DefaultNames.ObjectNameMapping[entityName].TableName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return DefaultNames.ObjectNameMapping[entityName].ColumnNames?[propertyName] ?? throw new InvalidDataException("Define column names for PG.");
  }
}