using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IStorageResolver _storageResolver = storageResolver;
  protected ITestRepositoryModule ReadTestContext() => _storageResolver.ReadFromStorage<ITestRepositoryModule>();

  protected Task<Result> StorageEntityParallelAction(Func<ITestRepositoryModule, StorageEntityExecutorItem> executor, string hashSalt = "")
    => base.StorageEntityParallelAction(executor, hashSalt);
  
  protected Task<Result> StorageParallelAction(Func<ITestRepositoryModule, StorageExecutorItem> executor)
    => base.StorageParallelAction(executor);
}