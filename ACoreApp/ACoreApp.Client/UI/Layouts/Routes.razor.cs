using ACore.Blazor.Services.App;
using Microsoft.AspNetCore.Components.Routing;

namespace ACoreApp.Client.UI.Layouts;

public partial class Routes(IAppState? appState, IAppPagesConfiguration? appStartConfiguration)
{
  private IAppState AppState { get; set; } = appState ?? throw new ArgumentNullException(nameof(appState));
  private IAppPagesConfiguration AppStartConfiguration { get; set; } = appStartConfiguration ?? throw new ArgumentNullException(nameof(appStartConfiguration));

  protected override void OnInitialized()
  {
    base.OnInitialized();
    AppStartConfiguration.ApplyTranslations();
  }

  private Task OnNavigateAsync(NavigationContext args)
  {
    AppState.SetPage(args.Path);
    return Task.CompletedTask;
  }
}