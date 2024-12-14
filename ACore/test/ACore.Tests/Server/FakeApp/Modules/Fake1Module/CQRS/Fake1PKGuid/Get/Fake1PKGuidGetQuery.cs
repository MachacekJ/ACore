using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Get;

public class Fake1PKGuidGetQuery: Fake1Request<Result<Fake1PKGuidData[]>>
{
  
}