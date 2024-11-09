using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Models;

public class TestPKStringData
{
  public string Id { get; set; } = string.Empty;
  public string? Name { get; set; }
  
  internal static TestPKStringData Create(TestPKStringEntity entity)
    => entity.Adapt<TestPKStringData>();
}