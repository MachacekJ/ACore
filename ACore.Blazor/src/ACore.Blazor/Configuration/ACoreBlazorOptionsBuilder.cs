using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App;
using ACore.Configuration;

namespace ACore.Blazor.Configuration;

public class ACoreBlazorOptionsBuilder : ACoreOptionsBuilder
{
  private IAppPagesConfiguration?  _appPageConfiguration;
  private readonly LocalStorageModuleOptions _localStorage = new();

  public void AddAppPagesConfiguration(IAppPagesConfiguration pagesConfiguration)
  {
    _appPageConfiguration = pagesConfiguration;
  }

  public void AddLocalStorage(Action<LocalStorageModuleOptions>? action = null)
  {
    action?.Invoke(_localStorage);
  }

  public override ACoreBlazorOptions Build()
  {
    var res = new ACoreBlazorOptions();
    SetOptions(res);
    return res;
  }

  private void SetOptions(ACoreBlazorOptions options)
  {
    base.SetOptions(options);
    options.LocalStorage = _localStorage;
    options.AppPages = _appPageConfiguration ?? throw new NullReferenceException($"App Pages configuration is null - {nameof(IAppPagesConfiguration)}");
  }
}