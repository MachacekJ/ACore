using ACore.Results;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public abstract class SettingsDbModuleRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, SettingsDbModuleOptions settingsDbModuleOptions) :
  RepositoryRequestHandler<ISettingsDbModuleRepository, TRequest, TResponse>(repositoryResolver, settingsDbModuleOptions)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  protected Task<Result> RepositoryAction(Func<ISettingsDbModuleRepository, RepositoryExecutorItem> executor)
    => DeleteFromRepositories(executor);
}