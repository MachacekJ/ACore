using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Save;

internal class Fake1PKGuidSaveHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions) 
  : Fake1RequestHandler<Fake1PKGuidSaveCommand, Result>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result> Handle(Fake1PKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1SqlRepositoryImpl:
          var en = Fake1PKGuidEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem<Fake1PKGuidEntity>(en, storage, storage.SaveTestEntity<Fake1PKGuidEntity, Guid>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}