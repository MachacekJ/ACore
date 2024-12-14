// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

[Auditable(1)]
internal class Fake1PKLongEntity : PKLongEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public static Fake1PKLongEntity Create(Fake1PKLongData data)
    => ToEntity<Fake1PKLongEntity>(data);
}

