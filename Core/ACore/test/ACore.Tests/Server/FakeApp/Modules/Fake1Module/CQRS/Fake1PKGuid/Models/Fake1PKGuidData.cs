using System.ComponentModel.DataAnnotations;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;

public class Fake1PKGuidData
{
  public Guid Id { get; set; } = Guid.Empty;
  
  [MaxLength(20)]
  public string? Name { get; set; }
  
  internal static Fake1PKGuidData Create(Fake1PKGuidEntity entity)
    => entity.Adapt<Fake1PKGuidData>();
}