using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Save;

internal class Fake1PKStringSaveHandler(IRepositoryResolver repositoryResolver, IOptions<Fake1ModuleOptions> testModuleOptions)
  : Fake1RequestHandler<Fake1PKStringSaveCommand, Result>(repositoryResolver, testModuleOptions.Value)
{
  public override async Task<Result> Handle(Fake1PKStringSaveCommand request, CancellationToken cancellationToken)
  {
    return await SaveEntityToRepositories((storage) =>
    {
      switch (storage)
      {
        case Fake1SqlRepositoryImpl:
          var en = Fake1PKStringEntity.Create(request.Data);
          return new RepositoryEntityExecutorItem<Fake1PKStringEntity>(en, storage, storage.SaveTestEntity<Fake1PKStringEntity, string>(en));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}