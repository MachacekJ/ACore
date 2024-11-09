using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditDeleteCommand<T>, Result>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result> Handle(TestAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    return await StorageParallelAction((storage)
      => storage switch
      {
        TestModuleMongoRepositoryImpl => new StorageExecutorItem(storage.DeleteTestEntity<TestAuditEntity, ObjectId>((ObjectId)Convert.ChangeType(request.Id, typeof(ObjectId)))),
        _ => new StorageExecutorItem(storage.DeleteTestEntity<Repositories.SQL.Models.TestAuditEntity, int>((int)Convert.ChangeType(request.Id, typeof(int))))
      });
  }
}