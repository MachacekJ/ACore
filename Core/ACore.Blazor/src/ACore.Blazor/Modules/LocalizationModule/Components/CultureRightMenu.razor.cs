using ACore.Blazor.Configuration;
using ACore.Blazor.Services.App.Manager;
using ACore.Blazor.Services.App.Models;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.PanelBar.Models;

namespace ACore.Blazor.Modules.LocalizationModule.Components;

public partial class CultureRightMenu(IJSRuntime jsRuntime, IAppManager appManager)
{
  private readonly List<PanelBarItem> _rootItems = [];

  protected override void OnInitialized()
  {
    base.OnInitialized();
    foreach (var item in ACoreBlazorAvailableLanguage.AllSupportedLanguages)
    {
      _rootItems.Add(new PanelBarItem
      {
        Id = item.LCID,
        Text = item.Text,
        Icon = item.Icon,
        DataItem = item
      });
    }
  }

  public async Task LanguageChange(PanelBarItemClickEventArgs panelBarItemArgs)
  {
    appManager.RightMenu.HideRightMenu();

    var panelBarItem = panelBarItemArgs.Item as PanelBarItem ?? throw new Exception($"null {nameof(panelBarItemArgs)}.Item");
    var lang = panelBarItem.DataItem as BlazorLanguageItem ?? throw new Exception($"null {nameof(panelBarItemArgs)}.DataItem");


    // LoadingAppExtension.Start("Loading language 1/3");
    // await Task.Delay(1000);
    // LoadingAppExtension.Text("Loading language 2/3");
    // await Task.Delay(1000);
    // LoadingAppExtension.Text("Loading language 3/3");
    // await Task.Delay(1000);
    // LoadingAppExtension.Stop();

    await LocalizationAppExtension.NotifyLanguageChangeAsync(lang.LCID, Mediator, jsRuntime);
  }
}