using ACore.Server.Storages.Attributes;

namespace ACore.Server.Storages.CQRS.Handlers;

public class StorageEntityExecutor(object? entity, IStorage storage, Task task) : StorageExecutor(task)
{
  public bool WithHash => entity?.GetType().IsHashCheck() ?? false;
  public object? Entity => entity;

  public IStorage Storage => storage;
}

public class StorageEntityExecutor<T>(T entity, IStorage storage, Task task) :
  StorageEntityExecutor(entity, storage, task)
  where T : class;