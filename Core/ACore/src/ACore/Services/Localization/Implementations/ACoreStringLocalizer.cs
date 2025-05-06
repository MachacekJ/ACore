using System.Globalization;
using ACore.Modules.LocalizationModule.Configuration;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;


namespace ACore.Services.Localization.Implementations;

/// <summary>
/// Implementation of <see cref="Microsoft.Extensions.Localization.IStringLocalizer"/>
/// </summary>
public class ACoreStringLocalizer(Type contextId, IOptions<LocalizationModuleOptions> localizationOptions) : IStringLocalizer
{
  private static int CurrentLCID => CultureInfo.CurrentCulture.LCID;

  public LocalizedString this[string name] => Localize(name, null);
  public LocalizedString this[string name, params object[] arguments] => Localize(name, arguments);

  private Type ContextId { get; } = contextId;

  public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
  {
    throw new NotImplementedException();
  }

  private LocalizedString Localize(string name, object[]? args)
  {
    IACoreLocalizationItem? res2 = null;
    foreach (var repository in localizationOptions.Value.LocalizationRepositories)
    {
      var res = repository.GetLocalizationRecord(new ACoreLocalizationKeyItem(ContextId, name), CurrentLCID);
      if (res != null)
        res2 = res;
    }

    if (res2 == null)
      return new LocalizedString(name, $"{ContextId}:{CurrentLCID}:{name}", true);

    var translated = res2.Translation;
    if (args != null && args.Any())
    {
      translated = string.Format(translated, args);
    }

    return new LocalizedString(name, translated);
  }
}