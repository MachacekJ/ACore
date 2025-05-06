using ACore.Results;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;

public class SettingsDbGetHandler(IRepositoryResolver repositoryResolver,IOptions<SettingsDbModuleOptions> settingsDbModuleOptions)
  : SettingsDbModuleRequestHandler<SettingsDbGetQuery, Result<string?>>(repositoryResolver, settingsDbModuleOptions.Value)
{
  private readonly IRepositoryResolver _repositoryResolver = repositoryResolver;

  public override async Task<Result<string?>> Handle(SettingsDbGetQuery request, CancellationToken cancellationToken)
  {
    var repositoryImplementation = _repositoryResolver.ReadRepositoryContext<ISettingsDbModuleRepository>();
    var res= await repositoryImplementation.Setting_GetAsync(request.Key, request.IsRequired);
    return Result.Success(res);
  }
}