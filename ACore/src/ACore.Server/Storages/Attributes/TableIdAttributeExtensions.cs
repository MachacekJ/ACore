using ACore.Server.Modules.AuditModule.Attributes;

namespace ACore.Server.Storages.Attributes;

internal static class TableIdAttributeExtensions
{
  internal static TableIdAttribute? TableIdAttr(this Type entityEntry)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(TableIdAttribute));
    
    if (enableAuditAttribute is TableIdAttribute auditableAttribute)
      return auditableAttribute;

    return null;
  }
}