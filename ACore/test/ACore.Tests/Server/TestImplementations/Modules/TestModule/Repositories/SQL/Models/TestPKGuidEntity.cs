using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Models;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;

[Auditable(1)]
internal class TestPKGuidEntity : PKGuidEntity
{
  public string? Name { get; set; }
  
  public static TestPKGuidEntity Create(TestPKGuidData data)
    => ToEntity<TestPKGuidEntity>(data);
}