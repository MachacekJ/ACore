using System.ComponentModel.DataAnnotations;
using ACore.Attributes;
using ACore.Server.Repository.Attributes;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;

[SumHash]
[MongoCollectionName("fake1_no_audits")]
internal class Fake1NoAuditEntity : PKMongoEntity
{
  [MaxLength(200)]
  [BsonElement("name")]
  public string Name { get; set; } = string.Empty;

  [BsonElement("created")]
  public DateTime Created { get; set; }

  public static Fake1NoAuditEntity Create<TPK>(Fake1NoAuditData<TPK> data)
    => ToEntity<Fake1NoAuditEntity>(data);
}
