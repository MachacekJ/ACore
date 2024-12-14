using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more repositories like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="DefaultNames"/>.
/// </summary>
[Auditable(1)]
internal class Fake1AuditEntity: PKIntEntity
{
  [MaxLength(50)]
  public string Name { get; set; } = string.Empty;
  
  [MaxLength(50)]
  public string? NullValue { get; set; }
  
  [MaxLength(50)]
  public string? NullValue2 { get; set; }
  
  [MaxLength(50)]
  public string? NullValue3 { get; set; }

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }

  public static Fake1AuditEntity Create<T>(Fake1AuditData<T> data)
    => ToEntity<Fake1AuditEntity>(data);
}