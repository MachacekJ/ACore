using ACore.Server.Repository.Configuration;

namespace BlazorApp.Modules.CustomerModule.Configuration;

public class CustomerModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  public static CustomerModuleOptionsBuilder Default() => new();

  public CustomerModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    var defaultServerStorageOptions = defaultRepositories.Build();
    var res = new CustomerModuleOptions(defaultServerStorageOptions);
    SetBase(res);
    return res;
  }
}