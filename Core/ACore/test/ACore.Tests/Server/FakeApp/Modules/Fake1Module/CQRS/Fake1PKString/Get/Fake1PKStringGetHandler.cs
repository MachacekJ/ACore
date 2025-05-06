using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Get;

internal class Fake1PKStringGetHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1PKStringGetQuery, Result<Fake1PKStringData[]>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Fake1PKStringData[]>> Handle(Fake1PKStringGetQuery request, CancellationToken cancellationToken)
  {
    var res = await ReadFromRepository().GetAll<Fake1PKStringEntity>();
    var resType = res.ResultValue?.ConvertAll(Fake1PKStringData.Create).ToArray();
    return resType == null
      ? Result.Failure<Fake1PKStringData[]>(new InvalidOperationException())
      : Result.Success(resType);
  }
}