using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;

[Auditable(1)]
internal class TestPKStringEntity : PKStringEntity
{
  [MaxLength(20)]
  public string? Name { get; set; }
  
  public static TestPKStringEntity Create(TestPKStringData data)
    => ToEntity<TestPKStringEntity>(data);
}