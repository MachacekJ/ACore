namespace ACore.Server.Storages.CQRS.Handlers;

public class StorageExecutor(Task task)
{
  public Task Task => task;
}