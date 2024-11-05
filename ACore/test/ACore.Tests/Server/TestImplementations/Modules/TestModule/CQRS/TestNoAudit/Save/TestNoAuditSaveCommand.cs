using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;

public class TestNoAuditSaveCommand<TPK>(TestNoAuditData<TPK> data, string? sumHash): TestModuleCommandRequest<Result>(sumHash)
{
  public TestNoAuditData<TPK> Data => data;
}