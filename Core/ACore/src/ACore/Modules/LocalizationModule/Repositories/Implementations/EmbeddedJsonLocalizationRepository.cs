using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Modules.LocalizationModule.Repositories.Implementations;

public class EmbeddedJsonLocalizationRepository : BaseLocalizationRepository
{
  public override IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid)
  {
    return null;
  }

  public override IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid)
  {
    return [];
  }
}