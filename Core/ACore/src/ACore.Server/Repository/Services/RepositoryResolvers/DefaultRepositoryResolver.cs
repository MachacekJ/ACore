using ACore.Repository;
using ACore.Repository.Definitions.Models;

namespace ACore.Server.Repository.Services.RepositoryResolvers;

public class RepositoryImplementation(IDbRepository implementation, RepositoryModeEnum mode = RepositoryModeEnum.ReadWrite)
{
  public RepositoryModeEnum Mode => mode;
  public IDbRepository Implementation => implementation;
}

public class DefaultRepositoryResolver : IRepositoryResolver
{
  private readonly Dictionary<string, List<RepositoryImplementation>> _implementations = [];

  public async Task ConfigureRepository<TRepository>(RepositoryImplementation implementation)
    where TRepository : IRepository
  {
    var name = typeof(TRepository).Name;
    if (implementation == null)
      throw new Exception($"Cannot find any implementation of {name}.");


    if (implementation.Implementation is not TRepository)
      throw new Exception($"Cannot find any implementation of {name}.");
    
    if (_implementations.TryGetValue(name, out var list))
    {
      // Only one database mode type (write/read) is allowed for particular repositoy type. e.g.  2 database for repositoy in reading mode is not allowed.  
      if (implementation.Mode.HasFlag(RepositoryModeEnum.Read) &&
          list.Any(e => e.Mode.HasFlag(RepositoryModeEnum.Read)))
        throw new Exception($"For the type {Enum.GetName(RepositoryModeEnum.Read)} only one repositoy '{name}' is allowed.");
      
      if (implementation.Mode.HasFlag(RepositoryModeEnum.Write) &&
          list.Any(e => e.Mode.HasFlag(RepositoryModeEnum.Write)))
        throw new Exception($"For the type {Enum.GetName(RepositoryModeEnum.Write)} only one repositoy '{name}' is allowed.");
      
      list.Add(implementation);
    }
    else
      _implementations.Add(name, [implementation]);

    try
    {
      await implementation.Implementation.UpSchema();
    }
    catch (Exception e)
    {
      throw new Exception($"Cannot configure '{name}' repository.", e);
    }
  }

  public T ReadRepositoryContext<T>(RepositoryTypeEnum repositoryType = RepositoryTypeEnum.AllDb) where T : IRepository
    => AllRepositories<T>(RepositoryModeEnum.Read, repositoryType).First();

  public IEnumerable<T> WriteToAllRepositories<T>(RepositoryTypeEnum repositoryType = RepositoryTypeEnum.AllDb) where T : IRepository
  {
    return AllRepositories<T>(RepositoryModeEnum.Write, repositoryType);
  }

  public T WriteRepositoryContext<T>() where T : IRepository
  {
    return AllRepositories<T>(RepositoryModeEnum.Write).First();
  }

  private List<T> AllRepositories<T>(RepositoryModeEnum mode, RepositoryTypeEnum repositoryType = RepositoryTypeEnum.AllDb) where T : IRepository
  {
    if (!_implementations.TryGetValue(typeof(T).Name, out var repositoryImplementations))
      throw new Exception($"Repositoy module '{typeof(T).Name}' has no registered implementation.");

    var repositoyImplementationByMode = repositoryImplementations.Where(e => e.Mode.HasFlag(mode)).Select(repository => repository.Implementation).OfType<T>().ToList();

    if (repositoryType != RepositoryTypeEnum.AllDb)
      repositoyImplementationByMode = repositoyImplementationByMode.Where(a => a.RepositoryInfo.RepositoryType == repositoryType).ToList();

    if (repositoyImplementationByMode.Count > 0)
      return repositoyImplementationByMode;

    throw new Exception($"Repository module '{typeof(T).Name}' has no registered implementation.");
  }
}