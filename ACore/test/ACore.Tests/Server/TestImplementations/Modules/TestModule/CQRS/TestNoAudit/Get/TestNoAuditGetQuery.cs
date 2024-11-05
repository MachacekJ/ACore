using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Get;

public class TestNoAuditGetQuery<TPK> : TestModuleQueryRequest<Result<Dictionary<string, TestNoAuditData<TPK>>>>;