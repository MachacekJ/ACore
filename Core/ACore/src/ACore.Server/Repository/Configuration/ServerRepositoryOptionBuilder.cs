using ACore.Modules.Base.Configuration;
using ACore.Repository.Definitions.Models;
using ACore.Server.Repository.Configuration.RepositoryTypes;

namespace ACore.Server.Repository.Configuration;

public class ServerRepositoryOptionBuilder : ModuleOptionsBuilder
{
  private RepositoryTypeEnum? _repositoryTestRegistration;
  private RepositoryPGOptions? _repositoryPGOptions;
  private RepositoryMongoOptions? _repositoryMongoOptions;
  private RepositoryMemoryOptions? _repositoryMemoryOptions;

  public ServerRepositoryOptionBuilder AddPG(string readWriteConnectionString, string? readOnlyConnectionString = null)
  {
    _repositoryPGOptions = new RepositoryPGOptions(readWriteConnectionString, readOnlyConnectionString);
    return this;
  }

  public ServerRepositoryOptionBuilder AddMongo(string readWriteConnectionString, string collectionName, string? readOnlyConnectionString = null)
  {
    _repositoryMongoOptions = new RepositoryMongoOptions(readWriteConnectionString, collectionName, readOnlyConnectionString);
    return this;
  }

  public ServerRepositoryOptionBuilder AddMemoryDb()
  {
    _repositoryMemoryOptions = new RepositoryMemoryOptions();
    return this;
  }

  public ServerRepositoryOptionBuilder DefaultRepositoryType(RepositoryTypeEnum defaultRepository)
  {
    _repositoryTestRegistration = defaultRepository;
    return this;
  }

  public ServerRepositoryOptions Build()
  {
    var res = new ServerRepositoryOptions(String.Empty);
    SetBase(res);
    return res;
  }

  protected void SetBase(ServerRepositoryOptions moduleOptions)
  {
    base.SetBase(moduleOptions);
    if (_repositoryTestRegistration != null)
      moduleOptions.DefaultToRead = _repositoryTestRegistration.Value;
    if (_repositoryMemoryOptions != null)
      moduleOptions.MemoryDb = _repositoryMemoryOptions;
    if (_repositoryPGOptions != null)
      moduleOptions.PGDb = _repositoryPGOptions;
    if (_repositoryMongoOptions != null)
      moduleOptions.MongoDb = _repositoryMongoOptions;
  }
}