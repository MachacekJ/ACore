using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL;
using MediatR;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler<TPK>(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditSaveCommand<TPK>, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand<TPK> request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleMongoRepositoryImpl:
          var enMongo = TestNoAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<TestNoAuditEntity, ObjectId>(enMongo));
        case TestModuleSqlRepositoryImpl:
          var en = Repositories.SQL.Models.TestNoAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem<Repositories.SQL.Models.TestNoAuditEntity>(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.TestNoAuditEntity, int>(en, request.SumHash));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}