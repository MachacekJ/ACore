using System.ComponentModel.DataAnnotations;
using ACore.Attributes;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.HashCheckSum.FakeClasses;

[SumHash]
public class WorkWithHashEntity : PKIntEntity
{
  [NotAuditable]
  [MaxLength(10)]
  public string? TestProp { get; set; }
  
  [NotIncludeToSumHash]
  [MaxLength(10)]
  public string? ExcludeFromHash { get; set; }
}