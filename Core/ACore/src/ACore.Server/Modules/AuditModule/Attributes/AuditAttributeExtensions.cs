namespace ACore.Server.Modules.AuditModule.Attributes;

internal static class AuditAttributeExtensions
{
  internal static AuditableAttribute? GetAuditableAttr(this Type entityEntry)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(AuditableAttribute));

    if (enableAuditAttribute is AuditableAttribute auditableAttribute)
      return auditableAttribute;

    return null;
  }

  internal static bool IsPropertyAuditable(this Type entityType, string propName)
  {
    var auditableAttribute = entityType.GetAuditableAttr();
    if (auditableAttribute == null)
      return false;

    var propertyInfo = entityType.GetProperty(propName);
    if (propertyInfo == null)
      throw new Exception($"Unknown property '{propName}' on type '{entityType.Name}'");

    return !Attribute.IsDefined(propertyInfo, typeof(NotAuditableAttribute));
  }
}