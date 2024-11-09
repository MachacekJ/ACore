using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories;

public interface ITestRepositoryModule : IRepository
{
  Task<RepositoryOperationResult> SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>;

  Task<RepositoryOperationResult> DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>;

  DbSet<TEntity> DbSet<TEntity, TPK>()  
    where TEntity : PKEntity<TPK>;
}