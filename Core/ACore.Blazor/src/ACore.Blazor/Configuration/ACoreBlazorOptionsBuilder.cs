using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App;

namespace ACore.Blazor.Configuration;

public class ACoreBlazorOptionsBuilder
{
  private IAppSettings?  _appPageConfiguration;
  private readonly LocalStorageModuleOptions _localStorage = new();

  private string BaseAddress { get; set; } = string.Empty;
  
public void AddPageSettings(IAppSettings settings)
  {
    _appPageConfiguration = settings;
  }

  public void AddLocalStorage(Action<LocalStorageModuleOptions>? action = null)
  {
    action?.Invoke(_localStorage);
  }

  public void SetBaseAddress(string baseAddress)
  {
    BaseAddress = baseAddress;
  }

  public ACoreBlazorOptions Build()
  {
    var res = new ACoreBlazorOptions();
    SetOptions(res);
    return res;
  }

  private void SetOptions(ACoreBlazorOptions options)
  {
    options.BaseAddress = BaseAddress;
    options.LocalStorage = _localStorage;
    options.AppPages = _appPageConfiguration ?? throw new NullReferenceException($"App Pages configuration is null - {nameof(IAppSettings)}");
  }
}