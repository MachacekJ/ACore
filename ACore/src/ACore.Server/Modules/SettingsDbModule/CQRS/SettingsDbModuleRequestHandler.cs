using ACore.Base.CQRS.Results;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public abstract class SettingsDbModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) :
  StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  protected Task<Result> PerformWriteAction(Func<ISettingsDbModuleStorage, DeleteProcessExecutor> executor)
    => base.PerformWriteAction(executor);
}