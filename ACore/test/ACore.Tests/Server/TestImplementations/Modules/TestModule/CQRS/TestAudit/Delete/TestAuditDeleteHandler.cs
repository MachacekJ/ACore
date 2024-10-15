using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditDeleteCommand<T>, Result>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result> Handle(TestAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    return await StorageAction((storage)
      => storage switch
      {
        TestModuleMongoStorageImpl => new StorageExecutor(storage.DeleteTestEntity<TestAuditEntity, ObjectId>((ObjectId)Convert.ChangeType(request.Id, typeof(ObjectId)))),
        _ => new StorageExecutor(storage.DeleteTestEntity<Storages.SQL.Models.TestAuditEntity, int>((int)Convert.ChangeType(request.Id, typeof(int))))
      });
  }
}