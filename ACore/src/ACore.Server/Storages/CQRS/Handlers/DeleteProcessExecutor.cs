namespace ACore.Server.Storages.CQRS.Handlers;

public class DeleteProcessExecutor(Task task)
{
  public Task Task => task;
}