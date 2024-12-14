using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Models;
using Mapster;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

[Auditable(1)]
internal class Fake1ValueTypeEntity() : Fake1ValueTypeEntityBase<int>(PKIntEntity.EmptyId)
{
  public static Fake1ValueTypeEntity Create<TPK>(Fake1ValueTypeData<TPK> data)
  {
#pragma warning disable CS8603 // Possible null reference return.
    var config = TypeAdapterConfig<Fake1ValueTypeData<TPK>, Fake1ValueTypeEntity>.NewConfig()
      .Ignore(d => d.TimeSpan2).Config;
#pragma warning restore CS8603 // Possible null reference return.
    var res = ToEntity<Fake1ValueTypeEntity>(data, config);
    res.TimeSpan2 = data.TimeSpan2?.Ticks;
    return res;
  }
}