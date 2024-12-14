using ACore.Results;
using ACore.Server.Configuration;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Get;

internal class Fake1NoAuditGetHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<ACoreServerOptions> options, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1NoAuditGetQuery<TPK>, Result<Dictionary<string, Fake1NoAuditData<TPK>>>>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result<Dictionary<string, Fake1NoAuditData<TPK>>>> Handle(Fake1NoAuditGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    var saltForHash = options.Value.SaltForHash;
    var readFromRepository = ReadFromRepository();
    Dictionary<string, Fake1NoAuditData<TPK>>? resType = null;
    switch (readFromRepository)
    {
      case Fake1MongoRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<Fake1NoAuditEntity>();
        if (res.ResultValue != null)
          resType = new Dictionary<string, Fake1NoAuditData<TPK>>(res.ResultValue.Select(a => Fake1NoAuditData<TPK>.Create<TPK>(a, saltForHash)));
        break;
      }
      case Fake1SqlRepositoryImpl:
      {
        var res = await readFromRepository.GetAll<ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models.Fake1NoAuditEntity>();
        if (res.ResultValue != null)
          resType = new Dictionary<string, Fake1NoAuditData<TPK>>(res.ResultValue.Select(a => Fake1NoAuditData<TPK>.Create<TPK>(a, saltForHash)));
        break;
      }
      default:
        return Result.Failure<Dictionary<string, Fake1NoAuditData<TPK>>>(new NotImplementedException());
    }

    return resType == null
      ? Result.Failure<Dictionary<string, Fake1NoAuditData<TPK>>>(new InvalidOperationException())
      : Result.Success(resType);


    // var st = ReadFromRepository();
    // if (st is Fake1MongoRepositoryImpl)
    // {
    //   var allItemsResult = st.GetAll<Fake1NoAuditEntity>().Result;
    //   if (allItemsResult.IsFailure || allItemsResult.ResultValue == null)
    //     return
    //       // var dbMongo = st.DbSet<Fake1NoAuditEntity, ObjectId>() ?? throw new Exception();
    //       // var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
    //       var
    //   r = new Dictionary<string, Fake1NoAuditData<TPK>>(allItemsResult.Select(a => Fake1NoAuditData<TPK>.Create<TPK>(a, saltForHash)));
    //   return Result.Success(r);
    // }
    //
    // var db = ReadFromRepository().DbSet<Repositories.SQL.Models.Fake1NoAuditEntity, int>() ?? throw new Exception();
    // var allItems = await db.ToArrayAsync(cancellationToken);
    // var testData = new Dictionary<string, Fake1NoAuditData<TPK>>(allItems.Select(a => Fake1NoAuditData<TPK>.Create<TPK>(a, saltForHash)));
    // return Result.Success(testData);
  }
}