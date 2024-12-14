using ACore.Services.Localization.Models;

namespace ACore.Server.Modules.LocalizationModule.CQRS.LocalizationGet.Models;

public class LocalizationItemDataOut : ACoreLocalizationItem
{
  public LocalizationItemDataOut(Type contextId, string key, int lcid, string translation) : base(contextId, key, lcid, translation)
  {
  }

  public LocalizationItemDataOut(ACoreLocalizationKeyItem localizationKey, int lcid, string translation) : base(localizationKey, lcid, translation)
  {
  }
}