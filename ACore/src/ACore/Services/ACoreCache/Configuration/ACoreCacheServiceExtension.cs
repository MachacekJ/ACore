using Microsoft.Extensions.DependencyInjection;

namespace ACore.Services.ACoreCache.Configuration;

public static class ACoreCacheServiceExtension
{
  public static void AddACoreCacheModule(this IServiceCollection services, ACoreCacheOptions options)
  {
    if (options.MemoryCacheOptionAction != null)
      services.AddMemoryCache(options.MemoryCacheOptionAction);
    else
      services.AddMemoryCache();
    
    services.AddSingleton<IACoreCache, ACoreCache>();
  }
}