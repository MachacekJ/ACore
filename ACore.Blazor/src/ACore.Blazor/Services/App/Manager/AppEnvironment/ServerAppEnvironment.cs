using Microsoft.AspNetCore.Components;

namespace ACore.Blazor.Services.App.Manager.AppEnvironment;

public class ServerAppEnvironment(NavigationManager navigationManager) : IAppEnvironment
{
  public Task SetStartLanguage(int defaultLanguage)
  {
    return Task.CompletedTask;
  }

  public Task ChangeLanguage(int lcid)
  {
    var uri = new Uri(navigationManager.Uri)
      .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
    var query = $"?lcid={lcid}&" +
                $"redirectUri={Uri.EscapeDataString(uri)}";

    navigationManager.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);

    return Task.CompletedTask;
  }
}