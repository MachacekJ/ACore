using ACore.Server.Storages.Contexts.EF.Models;

namespace ACore.Server.Storages.CQRS.Handlers.Models;

public class StorageExecutorItem(Task<DatabaseOperationResult> task)
{
  public Task<DatabaseOperationResult> Task => task;
}