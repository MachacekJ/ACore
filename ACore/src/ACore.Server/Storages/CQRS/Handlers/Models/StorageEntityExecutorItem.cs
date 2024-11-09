using ACore.Extensions;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;

namespace ACore.Server.Storages.CQRS.Handlers.Models;

public class StorageEntityExecutorItem : StorageExecutorItem
{
  public StorageEntityExecutorItem(object entity, IRepository repository, Task<RepositoryOperationResult> task) : base(task)
  {
    if (!entity.GetType().IsSubclassOfRawGeneric(typeof(PKEntity<>)))
      throw new ArgumentException($"Entity '{entity.GetType().FullName}' must be a subclass of a PKEntity.");

    Entity = entity;
    Repository = repository;
  }
  
  public object Entity { get; }
  public IRepository Repository { get; }
}

public class StorageEntityExecutorItem<T>(T entity, IRepository repository, Task<RepositoryOperationResult> task) :
  StorageEntityExecutorItem(entity, repository, task)
  where T : class;