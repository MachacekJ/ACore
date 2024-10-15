using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
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

  protected Task<Result> StorageEntityAction(Func<ITestStorageModule, StorageEntityExecutor> executor, string hashSalt = "")
    => base.StorageEntityAction(executor, hashSalt);
  
  protected Task<Result> StorageAction(Func<ITestStorageModule, StorageExecutor> executor)
    => base.StorageAction(executor);
}