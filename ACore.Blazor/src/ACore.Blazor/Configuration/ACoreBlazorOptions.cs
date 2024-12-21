using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App;
using ACore.Configuration;

namespace ACore.Blazor.Configuration;

public class ACoreBlazorOptions : ACoreOptions
{
  public IAppPagesConfiguration AppPages { get; set; }
  public LocalStorageModuleOptions LocalStorage { get; set; } = new();
}