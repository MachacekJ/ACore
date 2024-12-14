using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Save;

internal class Fake1NoAuditSaveHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1NoAuditSaveCommand<TPK>, Result>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result> Handle(Fake1NoAuditSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1MongoRepositoryImpl:
          var enMongo = Fake1NoAuditEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<Fake1NoAuditEntity, ObjectId>(enMongo));
        case Fake1SqlRepositoryImpl:
          var en = Repositories.SQL.Models.Fake1NoAuditEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem<Repositories.SQL.Models.Fake1NoAuditEntity>(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.Fake1NoAuditEntity, int>(en, request.SumHash));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}