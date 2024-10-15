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

  protected async Task<Result> StorageEntityAction<TStorage>(Func<TStorage, StorageEntityExecutor> executor, string hashSalt = "")
    where TStorage : IStorage
  {
    var allTask = storageResolver.WriteToStorages<TStorage>()
      .Select(executor).ToList();

    await WaitForAllParallelTasks(allTask.OfType<StorageExecutor>()
                              ?? throw new ArgumentNullException($"{nameof(allTask)}"));

    return EntityResult.SuccessWithEntityData(allTask, hashSalt);
  }
  
  protected async Task<Result> StorageAction<TStorage>(Func<TStorage, StorageExecutor> executor)
    where TStorage : IStorage
  {
    var allTask = storageResolver.WriteToStorages<TStorage>()
      .Select(executor).ToList();

    await WaitForAllParallelTasks(allTask);

    return Result.Success();
  }
  
  private static async Task WaitForAllParallelTasks(IEnumerable<StorageExecutor> allTask)
  {
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
  }
}