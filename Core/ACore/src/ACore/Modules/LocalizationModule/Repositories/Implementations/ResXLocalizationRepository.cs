using System.Globalization;
using System.Resources;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Modules.LocalizationModule.Repositories.Implementations;

public class ResXLocalizationRepository : BaseLocalizationRepository
{
 
  private readonly Dictionary<Type, ResourceManager> _resources = new();

  public ResXLocalizationRepository()
  {
    var baseType = typeof(ILocXConfig);
    var allComponents = AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(s => s.GetTypes())
      .Where(p => baseType.IsAssignableFrom(p) && p is { IsInterface: false, IsAbstract: false, IsClass: true });

    foreach (var type in allComponents)
    {
      if (Activator.CreateInstance(type) is ILocXConfig { LocX: not null } componentConfig)
        _resources.Add(componentConfig.LocX, new ResourceManager(componentConfig.LocX));
    }
  }

  public override IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid)
  {
    IACoreLocalizationItem? result = null;

    if (!_resources.TryGetValue(localizationKey.ContextId, out var resource))
      return result;

    var culture = new CultureInfo(lcid);
    var res = resource.GetString(localizationKey.Key, culture);
    if (res == null)
      return result;

    var item = new ACoreLocalizationItem(localizationKey, lcid, res);
    return item;
  }

  public override IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid)
  {
    throw new NotImplementedException();
  }
}

