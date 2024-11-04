using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Get;

public class TestAuditGetQuery<T>: TestModuleRequest<Result<TestAuditData<T>[]>>;