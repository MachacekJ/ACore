using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler<TPK>(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand<TPK>, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await PerformWriteActionWithData((storage) =>
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = Storages.Mongo.Models.TestValueTypeEntity.Create(request.Data);
          return new SaveProcessExecutor(enMongo, storage, storage.SaveTestEntity<Storages.Mongo.Models.TestValueTypeEntity, ObjectId>(enMongo));
        case TestModuleSqlStorageImpl:
          var en = TestValueTypeEntity.Create(request.Data);
          return new SaveProcessExecutor(en, storage, storage.SaveTestEntity<TestValueTypeEntity, int>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}