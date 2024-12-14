using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;

public class Fake1PKLongData
{
  public long Id { get; set; }
  public string Name { get; set; }
  public string NotAuditableColumn { get; set; }
  public DateTime Created { get; set; }

  internal static Fake1PKLongData Create(Fake1PKLongEntity entity)
    => entity.Adapt<Fake1PKLongData>();
}