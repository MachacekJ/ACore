using Microsoft.Extensions.Localization;

namespace ACore.Services.Localization.Models;

public class ACoreLocalizationKeyItem(Type contextId, string key)
{
  public Type ContextId { get; } = contextId;
  public string Key { get; } = key;
}

public static class ACoreLocalizationKeyItemExtensions
{
  public static string GetString(this ACoreLocalizationKeyItem localizationKey, IStringLocalizerFactory factory)
  {
    var stringLocalizer = factory.Create(localizationKey.ContextId);
    return stringLocalizer[localizationKey.Key];
  }
}