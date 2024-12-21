using ACore.Blazor.Configuration;

namespace ACoreApp.Client.Configuration;

public static class ACoreAppServiceExtensions
{
  public static void AddACoreAppSharedConfiguration(this IServiceCollection services)
  {
    services.AddACoreBlazor(b =>
    {
      b.AddAppPagesConfiguration(new AppPagesConfiguration());
      b.AddLocalStorage();
    });
  }
}