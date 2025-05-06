using ACore.Repository;
using ACore.Repository.Definitions.Models;

namespace ACore.Server.Repository.Services.RepositoryResolvers;

public interface IRepositoryResolver
{
  Task ConfigureRepository<TRepository>(RepositoryImplementation implementation)
    where TRepository : IRepository;
  
  T ReadRepositoryContext<T>(RepositoryTypeEnum repositoryType = RepositoryTypeEnum.AllDb) where T : IRepository;
  IEnumerable<T> WriteToAllRepositories<T>(RepositoryTypeEnum repositoryType = RepositoryTypeEnum.AllDb) where T : IRepository;
  public T WriteRepositoryContext<T>() where T : IRepository;
}