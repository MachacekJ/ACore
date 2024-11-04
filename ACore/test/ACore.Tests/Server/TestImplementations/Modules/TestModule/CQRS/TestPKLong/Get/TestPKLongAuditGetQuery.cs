using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Get;

public class TestPKLongAuditGetQuery: TestModuleRequest<Result<TestPKLongData[]>>;