using ACore.Blazor.Services.App;
using ACore.Blazor.Services.App.Manager;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorApp.Client.UI.Layouts;

public partial class Routes(IAppManager? appState, IAppSettings? appStartConfiguration) : ComponentBase
{
  private IAppManager AppManager { get; set; } = appState ?? throw new ArgumentNullException(nameof(appState));
  private IAppSettings AppStartConfiguration { get; set; } = appStartConfiguration ?? throw new ArgumentNullException(nameof(appStartConfiguration));

  private Task OnNavigateAsync(NavigationContext args)
  {
    AppManager.Page.SetPage(args.Path);
    return Task.CompletedTask;
  }
}