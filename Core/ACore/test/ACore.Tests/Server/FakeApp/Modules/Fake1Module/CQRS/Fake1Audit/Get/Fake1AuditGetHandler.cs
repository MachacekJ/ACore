using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Get;

public class Fake1AuditGetHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1AuditGetQuery<TPK>,Result<Fake1AuditData<TPK>[]>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Fake1AuditData<TPK>[]>> Handle(Fake1AuditGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    var readFromRepository = ReadFromRepository();
    Fake1AuditData<TPK>[]? resType;
    switch (readFromRepository)
    {
      case Fake1MongoRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<Fake1AuditEntity>();
        resType = res.ResultValue?.ConvertAll(Fake1AuditData<TPK>.Create<TPK>).ToArray();
        break;
      }
      case Fake1SqlRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models.Fake1AuditEntity>();
        resType = res.ResultValue?.ConvertAll(Fake1AuditData<TPK>.Create<TPK>).ToArray();
        break;
      }
      default:
        return Result.Failure<Fake1AuditData<TPK>[]>(new NotImplementedException());
    }

    return resType == null
      ? Result.Failure<Fake1AuditData<TPK>[]>(new InvalidOperationException())
      : Result.Success(resType);
  }
}