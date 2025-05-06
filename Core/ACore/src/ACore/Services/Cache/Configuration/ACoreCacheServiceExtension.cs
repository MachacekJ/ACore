using ACore.Services.Cache.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Services.Cache.Configuration;

public static class ACoreCacheServiceExtension
{
  public static void AddACoreCacheModule(this IServiceCollection services, ACoreCacheOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.AddSingleton(myOptionsInstance);
    
    if (options.MemoryCacheOptionAction != null)
      services.AddMemoryCache(options.MemoryCacheOptionAction);
    else
      services.AddMemoryCache();
    
    services.AddSingleton<IACoreCache, ACoreMemoryCache>();
  }
}