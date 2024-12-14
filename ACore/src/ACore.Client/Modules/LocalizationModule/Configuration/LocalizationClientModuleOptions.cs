using ACore.Modules.Base.Configuration;
using ACore.Modules.LocalizationModule.Configuration;

namespace ACore.Client.Modules.LocalizationModule.Configuration;

public class LocalizationClientModuleOptions : LocalizationModuleOptions, IModuleOptions
{
  public string ModuleName => nameof(LocalizationModule);
  public bool IsActive => false;
  public Uri? BaseAddress { get; set; }
}