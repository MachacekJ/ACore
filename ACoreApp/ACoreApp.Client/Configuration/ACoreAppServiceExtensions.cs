using System.Reflection;
using ACore.Blazor.Configuration;

namespace ACoreApp.Client.Configuration;

public static class ACoreAppServiceExtensions
{
  public static void AddACoreAppSharedConfiguration(this IServiceCollection services, IEnumerable<Assembly> assemblies, string baseAddress = "")
  {
    services.AddACoreBlazor(b =>
    {
      b.SetBaseAddress(baseAddress);
      b.AddPageSettings(new AppSettings(assemblies));
      b.AddLocalStorage();
    });
  }
}