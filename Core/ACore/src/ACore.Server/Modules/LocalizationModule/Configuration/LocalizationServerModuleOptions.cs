using ACore.Modules.LocalizationModule.Configuration;
using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.LocalizationModule.Configuration;

public class LocalizationServerModuleOptions(LocalizationModuleOptions localizationModuleOptions, ServerRepositoryOptions defaultServerRepositoryOptions) : ServerRepositoryOptions(defaultServerRepositoryOptions,  nameof(LocalizationModule))
{
  public LocalizationModuleOptions LocalizationModuleOptions => localizationModuleOptions;
}