using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleSqlStorageImpl:
          var en = TestPKGuidEntity.Create(request.Data);
          return new StorageEntityExecutorItem<TestPKGuidEntity>(en, storage, storage.SaveTestEntity<TestPKGuidEntity, Guid>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}