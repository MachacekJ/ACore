﻿using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditSaveCommand<T>, Result>(storageResolver)
  where T : IConvertible
{
  public override async Task<Result> Handle(TestAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleMongoRepositoryImpl:
          var enMongo = TestAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem(enMongo, storage, storage.SaveTestEntity<TestAuditEntity, ObjectId>(enMongo));
        case TestModuleSqlRepositoryImpl:
          var en = Repositories.SQL.Models.TestAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem(en, storage, storage.SaveTestEntity<Repositories.SQL.Models.TestAuditEntity, int>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}