using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKString.Save;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleSqlRepositoryImpl:
          var en = TestPKStringEntity.Create(request.Data);
          return new StorageEntityExecutorItem<TestPKStringEntity>(en, storage, storage.SaveTestEntity<TestPKStringEntity, string>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}