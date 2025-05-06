using ACore.Modules.LocalizationModule.Repositories;
using ACore.Modules.LocalizationModule.Repositories.Implementations;

namespace ACore.Modules.LocalizationModule.Configuration;

public class LocalizationModuleOptions
{
  public IEnumerable<ILocalizationRepository> LocalizationRepositories { get; set; } = [new MemoryLocalizationRepository([])];
  public IEnumerable<string> SupportedLanguages { get; set; } = ["en"];
}