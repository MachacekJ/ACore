using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

[Auditable(1)]
internal class Fake1PKStringEntity : PKStringEntity
{
  [MaxLength(20)]
  public string? Name { get; set; }
  
  public static Fake1PKStringEntity Create(Fake1PKStringData data)
    => ToEntity<Fake1PKStringEntity>(data);
}