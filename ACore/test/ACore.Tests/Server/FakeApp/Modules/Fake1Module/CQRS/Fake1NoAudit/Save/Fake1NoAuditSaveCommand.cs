using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Save;

public class Fake1NoAuditSaveCommand<TPK>(Fake1NoAuditData<TPK> data, string? sumHash): Fake1CommandRequest<Result>(sumHash)
{
  public Fake1NoAuditData<TPK> Data => data;
}