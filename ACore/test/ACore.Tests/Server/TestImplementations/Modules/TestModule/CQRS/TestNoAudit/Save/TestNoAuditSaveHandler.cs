using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.Models;
using MediatR;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler<TPK>(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditSaveCommand<TPK>, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = Storages.Mongo.Models.TestNoAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<Storages.Mongo.Models.TestNoAuditEntity, ObjectId>(enMongo));
        case TestModuleSqlStorageImpl:
          var en = TestNoAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem<TestNoAuditEntity>(en, storage, storage.SaveTestEntity<TestNoAuditEntity, int>(en, request.SumHash));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}