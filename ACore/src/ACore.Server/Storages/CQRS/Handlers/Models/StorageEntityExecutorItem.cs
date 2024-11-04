using ACore.Extensions;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.Server.Storages.CQRS.Handlers.Models;

public class StorageEntityExecutorItem : StorageExecutorItem
{
  public StorageEntityExecutorItem(object entity, IStorage storage, Task<DatabaseOperationResult> task) : base(task)
  {
    if (!entity.GetType().IsSubclassOfRawGeneric(typeof(PKEntity<>)))
      throw new ArgumentException($"Entity '{entity.GetType().FullName}' must be a subclass of a PKEntity.");

    Entity = entity;
    Storage = storage;
  }
  
  public object Entity { get; }
  public IStorage Storage { get; }
}

public class StorageEntityExecutorItem<T>(T entity, IStorage storage, Task<DatabaseOperationResult> task) :
  StorageEntityExecutorItem(entity, storage, task)
  where T : class;