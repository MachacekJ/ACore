using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Get;

public class Fake1ValueTypeGetQuery<TPK>: Fake1QueryRequest<Result<Fake1ValueTypeData<TPK>[]>>
{
  
}