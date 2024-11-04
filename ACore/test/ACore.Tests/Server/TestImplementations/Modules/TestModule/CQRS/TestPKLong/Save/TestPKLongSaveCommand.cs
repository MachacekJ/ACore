using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Save;

public class TestPKLongSaveCommand(TestPKLongData data): TestModuleRequest<Result>
{
  public TestPKLongData Data => data;
}