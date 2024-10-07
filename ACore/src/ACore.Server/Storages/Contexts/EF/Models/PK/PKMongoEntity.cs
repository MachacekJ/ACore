using MongoDB.Bson;

namespace ACore.Server.Storages.Contexts.EF.Models.PK;

public abstract class PKMongoEntity() : PKEntity<ObjectId>(EmptyId)
{
  public static ObjectId NewId => ObjectId.GenerateNewId();
  public static ObjectId EmptyId => ObjectId.Empty;
}
