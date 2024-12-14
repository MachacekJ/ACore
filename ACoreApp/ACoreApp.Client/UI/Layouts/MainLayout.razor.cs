using ACore.Blazor.Services.App.Manager;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ACoreApp.Client.UI.Layouts;

public partial class MainLayout(IAppManager appManager, IJSRuntime jsRuntime, NavigationManager navigationManager)
{
  private bool _isInitialized;


  protected override async Task OnInitializedAsync()
  {
    if (RendererInfo.IsInteractive)
      await appManager.Init(navigationManager, jsRuntime, RendererInfo.Name);

    await Task.Delay(2000).ConfigureAwait(false);

    _isInitialized = true;
  }
}