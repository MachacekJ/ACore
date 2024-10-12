using System.Runtime.ExceptionServices;
using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using MediatR;

namespace ACore.Server.Storages.CQRS.Handlers;

public abstract class StorageRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

  protected async Task<Result> PerformWriteAction<TStorage>(Func<TStorage, SaveProcessExecutor> executor, string hashSalt = "")
    where TStorage : IStorage
  {
    var allTask = storageResolver.WriteStorages<TStorage>()
      .Select(executor).ToList();

    Task? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.Task));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }

    return DbSaveResult.SuccessWithData(allTask, hashSalt);
  }
  
  protected async Task<Result> PerformWriteAction<TStorage>(Func<TStorage, DeleteProcessExecutor> executor)
    where TStorage : IStorage
  {
    var allTask = storageResolver.WriteStorages<TStorage>()
      .Select(executor).ToList();

    Task? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.Task));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }

    return Result.Success();
  }
}