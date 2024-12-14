using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Get;

public class Fake1NoAuditGetQuery<TPK> : Fake1QueryRequest<Result<Dictionary<string, Fake1NoAuditData<TPK>>>>;