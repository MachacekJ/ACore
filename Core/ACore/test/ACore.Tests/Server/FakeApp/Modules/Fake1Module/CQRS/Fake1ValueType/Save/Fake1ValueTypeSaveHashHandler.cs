using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Save;

internal class Fake1ValueTypeSaveHashHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions) 
  : Fake1RequestHandler<Fake1ValueTypeSaveHashCommand<TPK>, Result>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result> Handle(Fake1ValueTypeSaveHashCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1MongoRepositoryImpl:
          var enMongo = Fake1ValueTypeEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<Fake1ValueTypeEntity, ObjectId>(enMongo));
        case Fake1SqlRepositoryImpl:
          var en = Repositories.SQL.Models.Fake1ValueTypeEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.Fake1ValueTypeEntity, int>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}