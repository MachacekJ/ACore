﻿using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAuditDeleteCommand<T>, Result>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result> Handle(TestAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var t = storage.DeleteTestEntity<TestAuditEntity, ObjectId>((ObjectId)Convert.ChangeType(request.Id, typeof(ObjectId)));
          allTask.Add(t);
          break;
        default:
          var t2 = storage.DeleteTestEntity<Storages.SQL.Models.TestAuditEntity, int>((int)Convert.ChangeType(request.Id, typeof(int)));
          allTask.Add(t2);
          break;
      }
    }
    await Task.WhenAll(allTask);
    return Result.Success();
  }
}