using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Get;

public class Fake1PKLongAuditGetQuery: Fake1Request<Result<Fake1PKLongData[]>>;