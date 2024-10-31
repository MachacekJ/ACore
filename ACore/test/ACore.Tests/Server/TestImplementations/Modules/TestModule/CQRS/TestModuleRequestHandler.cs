using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IStorageResolver _storageResolver = storageResolver;
  protected ITestStorageModule ReadTestContext() => _storageResolver.ReadFromStorage<ITestStorageModule>();

  protected Task<Result> StorageEntityParallelAction(Func<ITestStorageModule, StorageEntityExecutorItem> executor, string hashSalt = "")
    => base.StorageEntityParallelAction(executor, hashSalt);
  
  protected Task<Result> StorageParallelAction(Func<ITestStorageModule, StorageExecutorItem> executor)
    => base.StorageParallelAction(executor);
}