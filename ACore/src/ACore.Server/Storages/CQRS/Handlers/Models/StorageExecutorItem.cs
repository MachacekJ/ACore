namespace ACore.Server.Storages.CQRS.Handlers.Models;

public class StorageExecutorItem(Task task)
{
  public Task Task => task;
}