using ACore.Server.Storages.Contexts.EF.Models;

namespace ACore.Server.Storages.CQRS.Handlers.Models;

public class StorageExecutorItem(Task<RepositoryOperationResult> task)
{
  public Task<RepositoryOperationResult> Task => task;
}