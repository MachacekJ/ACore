using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Get;

internal class Fake1PKGuidGetHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions) 
  : Fake1RequestHandler<Fake1PKGuidGetQuery, Result<Fake1PKGuidData[]>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Fake1PKGuidData[]>> Handle(Fake1PKGuidGetQuery request, CancellationToken cancellationToken)
  {
    var res = await ReadFromRepository().GetAll<Fake1PKGuidEntity>();
    var resType = res.ResultValue?.ConvertAll(Fake1PKGuidData.Create).ToArray();
    return resType == null
      ? Result.Failure<Fake1PKGuidData[]>(new InvalidOperationException())
      : Result.Success(resType);
  }
}