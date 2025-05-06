using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Save;

public class Fake1PKStringSaveCommand(Fake1PKStringData data): Fake1Request<Result>
{
  public Fake1PKStringData Data => data;
}