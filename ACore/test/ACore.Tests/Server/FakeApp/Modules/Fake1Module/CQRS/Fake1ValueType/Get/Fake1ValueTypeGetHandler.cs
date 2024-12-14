using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Get;

internal class Fake1ValueTypeGetHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1ValueTypeGetQuery<TPK>, Result<Fake1ValueTypeData<TPK>[]>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Fake1ValueTypeData<TPK>[]>> Handle(Fake1ValueTypeGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    var readFromRepository = ReadFromRepository();
    Fake1ValueTypeData<TPK>[]? resType;
    switch (readFromRepository)
    {
      case Fake1MongoRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<Fake1ValueTypeEntity>();
        resType = res.ResultValue?.ConvertAll(Fake1ValueTypeData<TPK>.Create<TPK>).ToArray();
        break;
      }
      case Fake1SqlRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models.Fake1ValueTypeEntity>();
        resType = res.ResultValue?.ConvertAll(Fake1ValueTypeData<TPK>.Create<TPK>).ToArray();
        break;
      }
      default:
        return Result.Failure<Fake1ValueTypeData<TPK>[]>(new NotImplementedException());
    }

    return resType == null
      ? Result.Failure<Fake1ValueTypeData<TPK>[]>(new InvalidOperationException())
      : Result.Success(resType);
  }
}