using System.Runtime.ExceptionServices;
using ACore.Repository;
using ACore.Results;
using ACore.Server.Repository.Configuration;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using MediatR;

namespace ACore.Server.Repository.CQRS.Handlers;

/// <summary>
/// For parallel saving/(getting)deleting data to/from repository.
/// </summary>
public abstract class RepositoryRequestHandler<TRepository, TRequest, TResponse>(IRepositoryResolver repositoryResolver, ServerRepositoryOptions repositoryOptions) : IRequestHandler<TRequest, TResponse>
  where TRepository : IRepository
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

  protected TRepository ReadFromRepository() => repositoryResolver.ReadRepositoryContext<TRepository>(repositoryOptions.DefaultToRead);

  protected async Task<RepositoryResult> SaveEntityToRepositories(Func<TRepository, RepositoryEntityExecutorItem> executor, string sumHashSalt = "")
  {
    var allTask = repositoryResolver.WriteToAllRepositories<TRepository>()
      .Select(executor).ToList();

    await WaitForAllSerialTasks(allTask.OfType<RepositoryExecutorItem>()
                                ?? throw new ArgumentNullException($"{nameof(allTask)}"));

    return RepositoryResult.CreateResultWithEntity(allTask);
  }


  protected async Task<Result> DeleteFromRepositories(Func<TRepository, RepositoryExecutorItem> executor)
  {
    var allTask = repositoryResolver.WriteToAllRepositories<TRepository>()
      .Select(executor).ToList();

    await WaitForAllSerialTasks(allTask);

    return Result.Success();
  }

  private static async Task WaitForAllSerialTasks(IEnumerable<RepositoryExecutorItem> allTask)
  {
    Task<RepositoryOperationResult[]>? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.DatabaseExecutableTask));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }
  }
  
  private static async Task WaitForAllParallelTasks(IEnumerable<RepositoryExecutorItem> allTask)
  {
    Task<RepositoryOperationResult[]>? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.DatabaseExecutableTask));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }
  }
}