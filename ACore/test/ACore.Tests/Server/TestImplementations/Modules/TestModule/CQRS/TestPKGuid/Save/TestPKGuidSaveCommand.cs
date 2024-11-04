using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Save;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<Result>
{
  public TestPKGuidData Data => data;
}