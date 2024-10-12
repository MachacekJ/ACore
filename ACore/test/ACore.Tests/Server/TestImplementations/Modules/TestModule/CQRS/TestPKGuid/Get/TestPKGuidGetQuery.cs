using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Get;

public class TestPKGuidGetQuery: TestModuleRequest<Result<TestPKGuidData[]>>
{
  
}