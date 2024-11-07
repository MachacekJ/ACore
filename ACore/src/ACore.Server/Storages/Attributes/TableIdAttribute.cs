namespace ACore.Server.Storages.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TableIdAttribute(string idName) : Attribute
{
  public string IdName => idName;
}