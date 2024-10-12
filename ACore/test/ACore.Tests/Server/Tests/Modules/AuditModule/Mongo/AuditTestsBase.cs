using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Mongo;

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);

public class AuditTestsBase() : AuditModuleTestsBase(StorageTypeEnum.Mongo)
{
  protected string GetTableName(string entityName)
  {
    return DefaultNames.ObjectNameMapping[entityName].TableName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    var columnNames = DefaultNames.ObjectNameMapping[entityName].ColumnNames;
    if (columnNames != null && columnNames.TryGetValue(propertyName, out var columnName))
      return columnName;
    
    return propertyName;
  }

 
}