using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.NotAuditProp;

[Auditable(2)]
public class FakeNotAuditPropLongEntity : PKLongEntity
{
  [NotAuditable]
  [MaxLength(10)]
  public string? TestProp { get; set; }
}