using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Modules.LocalizationModule.Repositories.Implementations;

public class MemoryLocalizationRepository(IEnumerable<IACoreLocalizationItem> allTranslations) : BaseLocalizationRepository
{
  public override IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid)
    => allTranslations.FirstOrDefault(a => a.Key == localizationKey.Key && a.ContextId == localizationKey.ContextId && a.Lcid == lcid);

  public override IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid)
    => allTranslations.Where(a => a.ContextId == contextId && a.Lcid == lcid);
}