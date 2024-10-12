using ACore.Server.Storages.Definitions.Models;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;

public class AuditTestsBase() : AuditModuleTestsBase(StorageTypeEnum.MemoryEF)
{
  protected string GetTableName(string entityName)
  {
    return entityName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return propertyName;
  }
}