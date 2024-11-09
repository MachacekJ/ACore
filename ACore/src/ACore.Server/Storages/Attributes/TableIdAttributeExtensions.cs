namespace ACore.Server.Storages.Attributes;

public static class TableIdAttributeExtensions
{
  public static TableIdAttribute? TableIdAttr(this Type entityEntry)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(TableIdAttribute));
    
    if (enableAuditAttribute is TableIdAttribute auditableAttribute)
      return auditableAttribute;

    return null;
  }
}