﻿using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Get;

public class TestAuditGetHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditGetQuery<T>,Result<TestAuditData<T>[]>>(storageResolver)
{
  public override async Task<Result<TestAuditData<T>[]>> Handle(TestAuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    var st = ReadTestContext();
    if (st is TestModuleMongoStorageImpl)
    {
      var dbMongo = st.DbSet<TestAuditEntity, ObjectId>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      var r = allItemsM.Select(TestAuditData<T>.Create<T>).ToArray();
      return Result.Success(r);
    }

    var db = st.DbSet<Storages.SQL.Models.TestAuditEntity, int>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    var rr = allItems.Select(TestAuditData<T>.Create<T>).ToArray();
    return Result.Success(rr);
  }
}