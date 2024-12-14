using ACore.Server.Repository.Contexts.EF.Models.PK;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Repository.Contexts.Mongo.Models.PK;

/// <summary>
/// Primary key only for mongo.
/// </summary>
public abstract class PKMongoEntity() : PKEntity<ObjectId>(EmptyId)
{
  [BsonElement("_v")]
  public int Version { get; set; } = 1;
  
  public static ObjectId NewId => ObjectId.GenerateNewId();
  public static ObjectId EmptyId => ObjectId.Empty;
}
