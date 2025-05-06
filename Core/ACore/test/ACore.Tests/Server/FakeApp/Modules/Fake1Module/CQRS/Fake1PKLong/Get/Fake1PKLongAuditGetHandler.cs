using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Get;

internal class Fake1PKLongAuditGetHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1PKLongAuditGetQuery, Result<Fake1PKLongData[]>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Fake1PKLongData[]>> Handle(Fake1PKLongAuditGetQuery request, CancellationToken cancellationToken)
  {
    var res = await ReadFromRepository().GetAll<Fake1PKLongEntity>();
    var resType = res.ResultValue?.ConvertAll(Fake1PKLongData.Create).ToArray();
    return resType == null
      ? Result.Failure<Fake1PKLongData[]>(new InvalidOperationException())
      : Result.Success(resType);
  }
}