using ACore.Server.Repository.Configuration;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;

public class Fake1ModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  public static Fake1ModuleOptionsBuilder Empty() => new();
  
  public Fake1ModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    var defaultServerStorageOptions = defaultRepositories.Build();
    var res = new Fake1ModuleOptions(defaultServerStorageOptions);
    SetBase(res);
    return res;
  }
}