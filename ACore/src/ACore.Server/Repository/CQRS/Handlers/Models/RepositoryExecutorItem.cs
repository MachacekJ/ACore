using ACore.Server.Repository.Results;

namespace ACore.Server.Repository.CQRS.Handlers.Models;

public class RepositoryExecutorItem(Task<RepositoryOperationResult> databaseExecutableTask)
{
  public Task<RepositoryOperationResult> DatabaseExecutableTask => databaseExecutableTask;
}