using ACore.Results;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveHandler(IRepositoryResolver repositoryResolver,IOptions<SettingsDbModuleOptions> settingsDbModuleOptions)
  : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>(repositoryResolver, settingsDbModuleOptions.Value)
{
  public override async Task<Result> Handle(SettingsDbSaveCommand request, CancellationToken cancellationToken)
  {
    return await DeleteFromRepositories((repository)
      => new RepositoryExecutorItem(repository.Setting_SaveAsync(request.Key, request.Value, request.IsSystem)));
  }
}