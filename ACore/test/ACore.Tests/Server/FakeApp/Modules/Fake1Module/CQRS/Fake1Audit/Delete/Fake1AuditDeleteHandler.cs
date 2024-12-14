using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Delete;

public class Fake1AuditDeleteHandler<T>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1AuditDeleteCommand<T>, Result>(repositoryResolver, testModuleOptions.Value)
  where T : IConvertible

{
  public override async Task<Result> Handle(Fake1AuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    return await DeleteFromRepositories((storage)
      => storage switch
      {
        Fake1MongoRepositoryImpl => new RepositoryExecutorItem(storage.DeleteTestEntity<Fake1AuditEntity, ObjectId>((ObjectId)Convert.ChangeType(request.Id, typeof(ObjectId)))),
        _ => new RepositoryExecutorItem(storage.DeleteTestEntity<Repositories.SQL.Models.Fake1AuditEntity, int>((int)Convert.ChangeType(request.Id, typeof(int))))
      });
  }
}