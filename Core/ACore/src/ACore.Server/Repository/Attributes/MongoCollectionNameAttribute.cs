namespace ACore.Server.Repository.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MongoCollectionNameAttribute(string collectionName) : Attribute
{
  public string CollectionName => collectionName;
}