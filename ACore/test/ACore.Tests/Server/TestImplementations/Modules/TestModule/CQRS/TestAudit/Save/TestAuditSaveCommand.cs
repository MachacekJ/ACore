using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveCommand<TPK>(TestAuditData<TPK> data): TestModuleRequest<Result>
{
  public TestAuditData<TPK> Data => data;
}

