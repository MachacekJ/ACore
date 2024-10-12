using ACore.Server.Configuration;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

namespace ACore.Tests.Server.TestImplementations.Configuration;

public class ACoreTestOptions
{
  public ACoreServerOptions ACoreServerOptions { get; init; } = new();
  public TestModuleOptions TestModuleOptions { get; init; } = new();
}