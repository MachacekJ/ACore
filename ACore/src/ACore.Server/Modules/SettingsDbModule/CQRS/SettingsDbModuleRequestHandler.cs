using ACore.Models.Result;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public abstract class SettingsDbModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) :
  StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  protected Task<Result> StorageAction(Func<ISettingsDbModuleRepository, StorageExecutorItem> executor)
    => StorageParallelAction(executor);
}