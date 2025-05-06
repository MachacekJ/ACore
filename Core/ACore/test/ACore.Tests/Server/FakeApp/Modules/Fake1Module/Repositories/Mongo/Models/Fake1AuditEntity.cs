using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Attributes;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more repositories like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="SQL.PG.DefaultNames"/>.
/// </summary>
[Auditable(1)]
[MongoCollectionName("fake1_audits")]
public class Fake1AuditEntity : PKMongoEntity
{
  [MaxLength(50)]
  [BsonElement("name")]
  public string Name { get; set; } = string.Empty;

  [NotAuditable]
  [MaxLength(50)]
  [BsonElement("notAuditableColumn")]
  public string NotAuditableColumn { get; set; } = string.Empty;

  [BsonElement("created")]
  public DateTime Created { get; set; }

  [MaxLength(50)]
  [BsonElement("nullValue")]
  public string? NullValue { get; set; }

  [MaxLength(50)]
  [BsonElement("nullValue2")]
  public string? NullValue2 { get; set; }

  [MaxLength(50)]
  [BsonElement("nullValue3")]
  public string? NullValue3 { get; set; }

  public static Fake1AuditEntity Create<T>(Fake1AuditData<T> data)
    => ToEntity<Fake1AuditEntity>(data);
}