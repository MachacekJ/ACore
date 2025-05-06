using ACore.Services.Localization.Interfaces;

namespace ACore.Services.Localization.Models;

public class ACoreLocalizationItem(ACoreLocalizationKeyItem localizationKey, int lcid, string translation) : ACoreLocalizationKeyItem(localizationKey.ContextId, localizationKey.Key), IACoreLocalizationItem
{
  public ACoreLocalizationItem(Type contextId, string key, int lcid, string translation) : this(new ACoreLocalizationKeyItem(contextId, key), lcid, translation)
  {
  }

  public ACoreLocalizationKeyItem LocalizationKey { get; } = localizationKey;
  public int Lcid { get; } = lcid;
  public string Translation { get; } = translation;
}