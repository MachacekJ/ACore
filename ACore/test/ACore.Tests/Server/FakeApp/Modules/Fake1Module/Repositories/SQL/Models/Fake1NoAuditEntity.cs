using System.ComponentModel.DataAnnotations;
using ACore.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

[SumHash]
internal class Fake1NoAuditEntity : PKIntEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  public static Fake1NoAuditEntity Create<TPK>(Fake1NoAuditData<TPK> data)
    => ToEntity<Fake1NoAuditEntity>(data);
}
