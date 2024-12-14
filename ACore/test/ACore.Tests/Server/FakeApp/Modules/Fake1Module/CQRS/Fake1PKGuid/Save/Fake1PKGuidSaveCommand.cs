using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Save;

public class Fake1PKGuidSaveCommand(Fake1PKGuidData data): Fake1Request<Result>
{
  public Fake1PKGuidData Data => data;
}