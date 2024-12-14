using ACore.Results;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Save;

public class Fake1ValueTypeSaveHashCommand<TPK>(Fake1ValueTypeData<TPK> data): Fake1CommandRequest<Result>(null)
{
  public Fake1ValueTypeData<TPK> Data => data;
}