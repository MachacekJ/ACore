using ACore.Server.Repository.Attributes;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Server.Repository.Models.EntityEvent;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo.Models;

[MongoCollectionName("audits")]
internal class AuditMongoEntity : PKMongoEntity
{
  [BsonElement("oid")]
  public string ObjectId { get; set; }
  
  [BsonElement("c")]
  public List<AuditMongoValueEntity>? Columns { get; set; }

  [BsonElement("t")]
  public DateTime Created { get; set; }

  [BsonElement("s")]
  public EntityEventEnum EntityState { get; set; }

  [BsonElement("u")]
  public AuditMongoUserEntity User { get; set; }
}