using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Save;

public class Fake1PKLongSaveCommand(Fake1PKLongData data): Fake1Request<Result>
{
  public Fake1PKLongData Data => data;
}