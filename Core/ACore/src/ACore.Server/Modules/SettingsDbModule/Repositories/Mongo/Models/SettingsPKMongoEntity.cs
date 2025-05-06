using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Attributes;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;

[Auditable(1)]
[MongoCollectionName("settings")]
public class SettingsPKMongoEntity : PKMongoEntity
{
  [BsonElement("key")]
  [MaxLength(1024)]
  public required string Key { get; set; }
  
  [BsonElement("value")]
  [MaxLength(1024)]
  public required string Value { get; set; }
  
  [BsonElement("isSystem")]
  public bool? IsSystem { get; set; }
}