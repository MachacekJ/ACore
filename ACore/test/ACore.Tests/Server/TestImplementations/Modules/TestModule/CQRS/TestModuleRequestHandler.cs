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
  protected ITestStorageModule ReadTestContext() => _storageResolver.FirstReadOnlyStorage<ITestStorageModule>();

  protected Task<Result> PerformWriteAction(Func<ITestStorageModule, SaveProcessExecutor> executor, string hashSalt = "")
    => base.PerformWriteAction(executor, hashSalt);
  
  protected Task<Result> PerformWriteAction(Func<ITestStorageModule, DeleteProcessExecutor> executor)
    => base.PerformWriteAction(executor);
}