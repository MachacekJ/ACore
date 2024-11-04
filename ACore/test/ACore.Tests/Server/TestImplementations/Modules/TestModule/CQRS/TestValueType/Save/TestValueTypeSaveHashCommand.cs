using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Save;

public class TestValueTypeSaveCommand<TPK>(TestValueTypeData<TPK> data): TestModuleCommandRequest<Result>(null)
{
  public TestValueTypeData<TPK> Data => data;
}