using ACore.Models.Result;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Get;

internal class TestPKGuidGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidGetQuery, Result<TestPKGuidData[]>>(storageResolver)
{
  public override async Task<Result<TestPKGuidData[]>> Handle(TestPKGuidGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestPKGuidEntity, Guid>() ?? throw new Exception();
    var r = await db.Select(a => TestPKGuidData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}