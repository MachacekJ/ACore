using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Services.ACoreCache.Configuration;

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
    
    services.AddSingleton<IACoreCache, ACoreCache>();
  }
}