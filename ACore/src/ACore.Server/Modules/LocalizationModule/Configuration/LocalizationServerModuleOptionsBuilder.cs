using ACore.Modules.LocalizationModule.Configuration;
using ACore.Modules.LocalizationModule.Repositories;
using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.LocalizationModule.Configuration;

public class LocalizationServerModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  private readonly LocalizationModuleOptions _localizationModuleOptions = new();
  private readonly List<ILocalizationRepository> _localizationRepositories = [];


  public static LocalizationServerModuleOptionsBuilder Empty() => new();

  public void AddLocalizationRepositories(IEnumerable<ILocalizationRepository> repositories)
  {
    _localizationRepositories.AddRange(repositories);
  }

  public void SetSupportedLanguages(params string[] supportedLanguages)
  {
    _localizationModuleOptions.SupportedLanguages = supportedLanguages;
  }

  public LocalizationServerModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    _localizationModuleOptions.LocalizationRepositories = _localizationRepositories;
    
    var defaultServerRepositoryOptions = defaultRepositories.Build();
    var res = new LocalizationServerModuleOptions(_localizationModuleOptions, defaultServerRepositoryOptions);
    SetBase(res);
    return res;
  }
}