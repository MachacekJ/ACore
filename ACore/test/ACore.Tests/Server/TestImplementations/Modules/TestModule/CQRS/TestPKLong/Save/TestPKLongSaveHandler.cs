using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Save;

internal class TestPKLongSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKLongSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKLongSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageEntityAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleSqlStorageImpl:
          var en = TestPKLongEntity.Create(request.Data);
          return new StorageEntityExecutor<TestPKLongEntity>(en, storage, storage.SaveTestEntity<TestPKLongEntity, long>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
    
    // var allTask = new List<SaveProcessExecutor<TestPKLongEntity>>();
    // foreach (var storage in WriteTestContexts())
    // {
    //   if (storage is TestModuleSqlStorageImpl)
    //   {
    //     var en = TestPKLongEntity.Create(request.Data);
    //     allTask.Add(new SaveProcessExecutor<TestPKLongEntity>(en, storage, storage.SaveTestEntity<TestPKLongEntity, long>(en)));
    //   }
    //   else
    //     throw new Exception($"{nameof(TestPKLongSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    // }
    //
    // await Task.WhenAll(allTask.Select(e => e.Task));
    // return DbSaveResult.SuccessWithData(allTask);
  }
}