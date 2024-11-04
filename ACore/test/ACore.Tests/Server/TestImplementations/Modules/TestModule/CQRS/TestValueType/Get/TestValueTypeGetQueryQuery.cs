using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Get;

public class TestValueTypeGetQuery<TPK>: TestModuleQueryRequest<Result<TestValueTypeData<TPK>[]>>
{
  
}