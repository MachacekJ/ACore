﻿using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;

internal class TestValueTypeGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery, Result<TestValueTypeData[]>>(storageResolver)
{
  public override async Task<Result<TestValueTypeData[]>> Handle(TestValueTypeGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestValueTypeEntity, int>() ?? throw new Exception();
    var r= await db
      .Select(a => TestValueTypeData.Create(a))
      .ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}