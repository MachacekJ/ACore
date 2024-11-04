using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Save;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest<Result>
{
  public TestPKStringData Data => data;
}