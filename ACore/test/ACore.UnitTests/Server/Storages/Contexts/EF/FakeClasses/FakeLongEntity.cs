using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses;

public class FakeLongEntity : PKLongEntity
{
  [MaxLength(10)]
  public string? TestProp { get; set; }
}