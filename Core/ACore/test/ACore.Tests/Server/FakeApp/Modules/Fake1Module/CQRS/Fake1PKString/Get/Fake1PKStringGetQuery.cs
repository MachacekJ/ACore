using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Get;

public class Fake1PKStringGetQuery: Fake1Request<Result<Fake1PKStringData[]>>
{
  
}