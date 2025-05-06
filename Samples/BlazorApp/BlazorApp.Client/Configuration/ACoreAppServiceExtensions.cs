using System.Reflection;
using ACore.Blazor.Configuration;

namespace BlazorApp.Client.Configuration;

public static class BlazorAppServiceExtensions
{
  public static void AddBlazorAppSharedConfiguration(this IServiceCollection services, IEnumerable<Assembly> assemblies, string baseAddress = "")
  {
    services.AddACoreBlazor(b =>
    {
      b.SetBaseAddress(baseAddress);
      b.AddPageSettings(new AppSettings(assemblies));
      b.AddLocalStorage();
    });
  }
}