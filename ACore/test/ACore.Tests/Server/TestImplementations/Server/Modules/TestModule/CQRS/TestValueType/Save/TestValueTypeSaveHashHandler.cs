using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler<TPK>(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand<TPK>, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor>();

    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = Storages.Mongo.Models.TestValueTypeEntity.Create(request.Data);
          allTask.Add(new SaveProcessExecutor(enMongo, storage, storage.SaveTestEntity<Storages.Mongo.Models.TestValueTypeEntity, ObjectId>(enMongo)));
          break;
        default:
          var en = TestValueTypeEntity.Create(request.Data);
          allTask.Add(new SaveProcessExecutor(en, storage, storage.SaveTestEntity<TestValueTypeEntity, int>(en)));
          break;
      }
    }

    await Task.WhenAll(allTask.Select(t => t.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}