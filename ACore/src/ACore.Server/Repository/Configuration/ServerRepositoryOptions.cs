using ACore.Modules.Base.Configuration;
using ACore.Repository.Definitions.Models;
using ACore.Server.Repository.Configuration.RepositoryTypes;

namespace ACore.Server.Repository.Configuration;

public class ServerRepositoryOptions : ModuleOptions
{
  public override string ModuleName { get; }
  public RepositoryTypeEnum DefaultToRead { get; set; } = RepositoryTypeEnum.First;
  public RepositoryMongoOptions? MongoDb { get; set; }
  public RepositoryPGOptions? PGDb { get; set; }
  public RepositoryMemoryOptions? MemoryDb { get; set; }

  protected ServerRepositoryOptions(ServerRepositoryOptions defaultOptions, string moduleName)
  {
    ModuleName = moduleName;
    DefaultToRead = defaultOptions.DefaultToRead;
    MongoDb = defaultOptions.MongoDb;
    PGDb = defaultOptions.PGDb;
    MemoryDb = defaultOptions.MemoryDb;
  }

  public ServerRepositoryOptions(string moduleName)
  {
    ModuleName = moduleName;
  }
}