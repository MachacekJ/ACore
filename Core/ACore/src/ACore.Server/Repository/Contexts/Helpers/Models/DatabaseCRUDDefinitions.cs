namespace ACore.Server.Repository.Contexts.Helpers.Models;

public record DatabaseCRUDDefinitions<TEntity, TPK>(
  Func<TPK> NewIdFunc,
  Func<TPK, Task<TEntity?>> GetSavedDataFunc,
  Func<TEntity, Task> AddDataFunc,
  Func<TEntity, Task> UpdateDataFunc,
  Func<TPK, Task> DeleteDataFunc,
  Func<Task> SaveDataFunc);