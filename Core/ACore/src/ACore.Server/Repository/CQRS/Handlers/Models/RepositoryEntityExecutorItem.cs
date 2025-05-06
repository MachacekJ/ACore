using ACore.Extensions;
using ACore.Repository;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Results;

namespace ACore.Server.Repository.CQRS.Handlers.Models;

public class RepositoryEntityExecutorItem : RepositoryExecutorItem
{
  public RepositoryEntityExecutorItem(object entity, IRepository repository, Task<RepositoryOperationResult> databaseExecutableTask) : base(databaseExecutableTask)
  {
    if (!entity.GetType().IsSubclassOfRawGeneric(typeof(PKEntity<>)))
      throw new ArgumentException($"Entity '{entity.GetType().FullName}' must be a subclass of a PKEntity.");

    Entity = entity;
    Repository = repository;
  }
  
  public object Entity { get; }
  public IRepository Repository { get; }
}

public class RepositoryEntityExecutorItem<T>(T entity, IRepository repository, Task<RepositoryOperationResult> databaseExecutableTask) :
  RepositoryEntityExecutorItem(entity, repository, databaseExecutableTask)
  where T : class;