using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Save;

public class Fake1AuditSaveCommand<TPK>(Fake1AuditData<TPK> data): Fake1Request<Result>
{
  public Fake1AuditData<TPK> Data => data;
}

