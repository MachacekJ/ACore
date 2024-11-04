using Mapster;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Models;

public class TestAuditData<TPK>
{
  public TPK Id { get; set; } = default(TPK) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(TPK).Name}");
  public string Name { get; set; } = string.Empty;

  public string? NullValue { get; set; }
  public string? NullValue2 { get; set; }
  public string? NullValue3 { get; set; }
  public string NotAuditableColumn { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  internal static TestAuditData<TPK> Create<TPK>(Storages.SQL.Models.TestAuditEntity entity)
    => entity.Adapt<TestAuditData<TPK>>();

  internal static TestAuditData<TPK> Create<TPK>(TestAuditEntity entity)
    => entity.Adapt<TestAuditData<TPK>>();
}