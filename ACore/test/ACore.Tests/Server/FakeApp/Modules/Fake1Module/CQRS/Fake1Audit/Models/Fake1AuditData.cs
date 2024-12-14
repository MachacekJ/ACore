using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using Mapster;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;

public class Fake1AuditData<TPK>
{
  public TPK Id { get; set; } = default(TPK) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(TPK).Name}");
  public string Name { get; set; } = string.Empty;

  public string? NullValue { get; set; }
  public string? NullValue2 { get; set; }
  public string? NullValue3 { get; set; }
  public string NotAuditableColumn { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  internal static Fake1AuditData<T> Create<T>(Repositories.SQL.Models.Fake1AuditEntity entity)
    => entity.Adapt<Fake1AuditData<T>>();

  internal static Fake1AuditData<T> Create<T>(Fake1AuditEntity entity)
    => entity.Adapt<Fake1AuditData<T>>();
}