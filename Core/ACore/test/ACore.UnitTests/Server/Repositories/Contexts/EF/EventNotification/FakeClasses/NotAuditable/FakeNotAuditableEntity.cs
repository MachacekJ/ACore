using System.ComponentModel.DataAnnotations;
using ACore.Server.Repository.Contexts.EF.Models.PK;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.NotAuditable;

public class FakeNotAuditableEntity : PKLongEntity
{
  [MaxLength(10)]
  public string? TestProp { get; set; }
}