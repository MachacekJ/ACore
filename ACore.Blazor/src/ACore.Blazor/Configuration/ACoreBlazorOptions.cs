using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App;

namespace ACore.Blazor.Configuration;

public class ACoreBlazorOptions
{
  public IAppSettings AppPages { get; set; }
  
  public string? BaseAddress { get; set; }
  public LocalStorageModuleOptions LocalStorage { get; set; } = new();
}