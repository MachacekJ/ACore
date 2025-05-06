using ACore.Repository.Models;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Modules.LocalizationModule.Repositories.Implementations;

public abstract class BaseLocalizationRepository : ILocalizationRepository
{
  public abstract IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid);
  public abstract IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid);

  public RepositoryInfo RepositoryInfo => new(nameof(ILocalizationRepository));
}