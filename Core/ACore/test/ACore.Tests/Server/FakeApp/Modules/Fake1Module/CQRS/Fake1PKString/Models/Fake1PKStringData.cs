using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;

public class Fake1PKStringData
{
  public string Id { get; set; } = string.Empty;
  public string? Name { get; set; }
  
  internal static Fake1PKStringData Create(Fake1PKStringEntity entity)
    => entity.Adapt<Fake1PKStringData>();
}