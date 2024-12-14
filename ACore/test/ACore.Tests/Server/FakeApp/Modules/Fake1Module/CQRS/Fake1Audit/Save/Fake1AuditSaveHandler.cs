using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Save;

public class Fake1AuditSaveHandler<T>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1AuditSaveCommand<T>, Result>(repositoryResolver, testModuleOptions.Value)
  where T : IConvertible
{
  public override async Task<Result> Handle(Fake1AuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1MongoRepositoryImpl:
          var enMongo = Fake1AuditEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<Fake1AuditEntity, ObjectId>(enMongo));
        case Fake1SqlRepositoryImpl:
          var en = Repositories.SQL.Models.Fake1AuditEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.Fake1AuditEntity, int>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}