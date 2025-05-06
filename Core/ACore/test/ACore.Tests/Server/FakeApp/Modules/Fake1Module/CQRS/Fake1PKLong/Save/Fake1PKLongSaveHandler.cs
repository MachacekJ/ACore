using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Save;

internal class Fake1PKLongSaveHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1PKLongSaveCommand, Result>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result> Handle(Fake1PKLongSaveCommand request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1SqlRepositoryImpl:
          var en = Fake1PKLongEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem<Fake1PKLongEntity>(en, storage, storage.SaveTestEntity<Fake1PKLongEntity, long>(en));
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