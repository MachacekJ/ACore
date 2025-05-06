using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Get;

public class Fake1AuditGetQuery<T>: Fake1Request<Result<Fake1AuditData<T>[]>>;