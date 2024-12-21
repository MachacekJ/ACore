using ACore.Models.Result;
using ACore.Server.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Get;

internal class TestNoAuditGetHandler<TPK>(IStorageResolver storageResolver, IOptions<ACoreServerOptions> options) : TestModuleRequestHandler<TestNoAuditGetQuery<TPK>, Result<Dictionary<string, TestNoAuditData<TPK>>>>(storageResolver)
{
  public override async Task<Result<Dictionary<string, TestNoAuditData<TPK>>>> Handle(TestNoAuditGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    var saltForHash = options.Value.SaltForHash;
    var st = ReadTestContext();
    if (st is TestModuleMongoRepositoryImpl)
    {
      var dbMongo = st.DbSet<TestNoAuditEntity, ObjectId>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      var r =  new Dictionary<string, TestNoAuditData<TPK>>(allItemsM.Select(a=>TestNoAuditData<TPK>.Create<TPK>(a, saltForHash)));
      return Result.Success(r);
    }

    var db = ReadTestContext().DbSet<Repositories.SQL.Models.TestNoAuditEntity, int>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken);
    var testData = new Dictionary<string, TestNoAuditData<TPK>>(allItems.Select(a => TestNoAuditData<TPK>.Create<TPK>(a, saltForHash)));
    return Result.Success(testData);
  }
}