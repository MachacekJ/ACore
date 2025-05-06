using ACore.Modules.LocalizationModule.Repositories;
using ACore.Repository.Models;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Client.Modules.LocalizationModule.Repositories.Implemantations;

public class LocalizationHttpRepositoryImpl : ILocalizationRepository
{
  public RepositoryInfo RepositoryInfo => new(nameof(ILocalizationRepository));

  public IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid)
  {
    throw new NotImplementedException();
  }
}