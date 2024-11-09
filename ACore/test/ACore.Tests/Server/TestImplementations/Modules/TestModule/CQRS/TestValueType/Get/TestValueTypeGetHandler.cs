using ACore.Models.Result;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Get;

internal class TestValueTypeGetHandler<TPK>(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery<TPK>, Result<TestValueTypeData<TPK>[]>>(storageResolver)
{
  public override async Task<Result<TestValueTypeData<TPK>[]>> Handle(TestValueTypeGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    var st = ReadTestContext();
    if (st is TestModuleMongoRepositoryImpl)
    {
      var dbMongo = st.DbSet<TestValueTypeEntity, ObjectId>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      var r = allItemsM.Select(TestValueTypeData<TPK>.Create<TPK>).ToArray();
      return Result.Success(r);
    }

    var db = st.DbSet<Repositories.SQL.Models.TestValueTypeEntity, int>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    var rr = allItems.Select(TestValueTypeData<TPK>.Create<TPK>).ToArray();
    return Result.Success(rr); 
  }
}