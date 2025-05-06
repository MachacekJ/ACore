using ACore.Server.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;

namespace ACore.Tests.Server.FakeApp.Configuration;

public class FakeAppOptions : ACoreServerOptions
{
  public Fake1ModuleOptions? TestModuleOptions { get; set; }
}