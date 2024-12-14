using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  public static SettingsDbModuleOptionsBuilder Empty() => new();

  public SettingsDbModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    var defaultServerRepositoryOptions = defaultRepositories.Build();
    var res = new SettingsDbModuleOptions(defaultServerRepositoryOptions);
    SetBase(res);
    return res;
  }
}