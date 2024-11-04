using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Get;

public class TestPKStringGetQuery: TestModuleRequest<Result<TestPKStringData[]>>
{
  
}