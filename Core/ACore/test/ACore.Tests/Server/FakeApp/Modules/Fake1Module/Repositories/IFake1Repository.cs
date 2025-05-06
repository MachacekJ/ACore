using ACore.Results;
using ACore.Server.Repository;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Results;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories;

public interface IFake1Repository : IDbRepository
{
  Task<RepositoryOperationResult> SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>;

  Task<RepositoryOperationResult> DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>;

  Task<Result<List<TEntity>>> GetAll<TEntity>()
    where TEntity : class;
}