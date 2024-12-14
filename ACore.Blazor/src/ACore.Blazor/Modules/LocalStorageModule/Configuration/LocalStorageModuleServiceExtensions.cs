using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Blazor.Modules.LocalStorageModule.Configuration;

public static class LocalStorageModuleServiceExtensions
{
  public static void AddLocalStorageModule(this IServiceCollection services, LocalStorageModuleOptions options)
  {
    services.AddBlazoredLocalStorage();
  }
}