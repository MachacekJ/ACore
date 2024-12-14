using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;

[Auditable(1)]
internal class Fake1PKGuidEntity : PKGuidEntity
{
  public string? Name { get; set; }
  
  public static Fake1PKGuidEntity Create(Fake1PKGuidData data)
    => ToEntity<Fake1PKGuidEntity>(data);
}