using ACore.Results;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Delete;

public class Fake1AuditDeleteCommand<T>(T id): Fake1Request<Result>
{
  public T Id => id;
}