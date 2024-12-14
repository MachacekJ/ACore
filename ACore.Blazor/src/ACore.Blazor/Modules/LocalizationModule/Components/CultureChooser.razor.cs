using System.Globalization;
using ACore.Blazor.Configuration;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ACore.Blazor.Modules.LocalizationModule.Components;

public partial class CultureChooser : ACoreComponentBase
{
  private BlazorLanguageItem _value = ACoreBlazorAvailableLanguage.AllSupportedLanguages.First();

  private CultureInfo Culture => CultureInfo.CurrentCulture;
  
  protected override void OnInitialized()
  {
    base.OnInitialized();
    LocalizationAppExtension.OnLocalizationChangeAsync += LocalizationAppLocalizationLocalizationAppLocalizationLocalizationAppChangeAsync;
    InitLocal();
  }

  public override void Dispose()
  {
    base.Dispose();
    LocalizationAppExtension.OnLocalizationChangeAsync -= LocalizationAppLocalizationLocalizationAppLocalizationLocalizationAppChangeAsync;
  }

  private Task LocalizationAppLocalizationLocalizationAppLocalizationLocalizationAppChangeAsync()
  {
    InitLocal();
    StateHasChanged();
    return Task.CompletedTask;
  }

  private void ShowContextMenu(MouseEventArgs e)
  {
    AppManager.RightMenu.ShowRightMenu(typeof(CultureRightMenu));
  }

  private void InitLocal()
  {
    var result = ACoreBlazorAvailableLanguage.AllSupportedLanguages.FirstOrDefault(a => a.Name == Culture.Name);
    if (result == null)
    {
      _value = ACoreBlazorAvailableLanguage.AllSupportedLanguages.First();
      return;
    }

    _value = result;
  }
}