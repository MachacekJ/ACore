namespace ACore.Server.Storages.CQRS.Handlers;

public class StorageExecutorItem(Task task)
{
  public Task Task => task;
}