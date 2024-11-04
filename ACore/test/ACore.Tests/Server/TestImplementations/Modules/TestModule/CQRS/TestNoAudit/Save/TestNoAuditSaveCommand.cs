using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;

public class TestNoAuditSaveCommand(TestNoAuditData data, string? sumHash): TestModuleCommandRequest<Result>(sumHash)
{
  public TestNoAuditData Data => data;
}