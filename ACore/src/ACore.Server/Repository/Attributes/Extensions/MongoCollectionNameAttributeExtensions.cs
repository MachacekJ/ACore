using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Repository.Attributes.Extensions;

public static class MongoCollectionNameAttributeExtensions
{
  public static string GetCollectionName(this Type entityType)
  {
    return GetMongoCollectionNameAttr(entityType)?.CollectionName ?? throw new ArgumentNullException($"Missing MongoCollectionNameAttribute on entity type {entityType.Name}.");
  }

  private static MongoCollectionNameAttribute? GetMongoCollectionNameAttr(this Type entityEntry)
  {
    var customAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(MongoCollectionNameAttribute));

    if (customAttribute is MongoCollectionNameAttribute mongoCollectionNameAttribute)
      return mongoCollectionNameAttribute;

    return null;
  }
  
  public static string GetMongoEntityName(this Type type, string propertyName)
  {
    //var type = typeof(TEntity);
    var bCustomAttributes = type.GetProperty(propertyName)?.GetCustomAttributes(typeof(BsonElementAttribute), true);
    if (bCustomAttributes is { Length: > 0 } && bCustomAttributes[0] is BsonElementAttribute attribute)
      return attribute.ElementName;

    return propertyName;
  }
}