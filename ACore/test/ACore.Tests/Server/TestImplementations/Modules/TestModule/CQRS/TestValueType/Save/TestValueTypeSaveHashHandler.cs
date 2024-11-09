using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler<TPK>(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand<TPK>, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleMongoRepositoryImpl:
          var enMongo = TestValueTypeEntity.Create(request.Data);
          return new StorageEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<TestValueTypeEntity, ObjectId>(enMongo));
        case TestModuleSqlRepositoryImpl:
          var en = Repositories.SQL.Models.TestValueTypeEntity.Create(request.Data);
          return new StorageEntityExecutorItem(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.TestValueTypeEntity, int>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}