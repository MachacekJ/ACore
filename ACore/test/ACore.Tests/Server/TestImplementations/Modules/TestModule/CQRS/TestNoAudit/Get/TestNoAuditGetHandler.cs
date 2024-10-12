using ACore.Base.CQRS.Results;
using ACore.Server.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Get;

internal class TestNoAuditGetHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> options) : TestModuleRequestHandler<TestNoAuditGetQuery, Result<Dictionary<string, TestNoAuditData>>>(storageResolver)
{
  public override async Task<Result<Dictionary<string, TestNoAuditData>>> Handle(TestNoAuditGetQuery request, CancellationToken cancellationToken)
  {
    var saltForHash = options.Value.ACoreOptions.SaltForHash;
    var db = ReadTestContext().DbSet<TestNoAuditEntity, int>() ?? throw new Exception();
    var testData = new Dictionary<string, TestNoAuditData>(await db.Select(a => TestNoAuditData.Create(a, saltForHash)).ToArrayAsync(cancellationToken: cancellationToken));
    return Result.Success(testData);
  }
}