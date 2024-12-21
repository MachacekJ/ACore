using ACore.Server.Configuration;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

namespace ACore.Tests.Server.TestImplementations.Configuration;

public class ACoreTestOptions : ACoreServerOptions
{
  public TestModuleOptions TestModuleOptions { get; set; } = new();
}