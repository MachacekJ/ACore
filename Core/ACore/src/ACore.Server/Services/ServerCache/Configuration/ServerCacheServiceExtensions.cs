using ACore.Server.Services.ServerCache.Implementations;
using ACore.Services.Cache.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Services.ServerCache.Configuration;

public static class ServerCacheServiceExtensions
{
  public static void AddServerCache(this IServiceCollection services, ServerCacheOptions? options)
  {
    if (options == null)
    {
      services.TryAddSingleton<IServerCache, ServerCacheDefault>();
      return;
    }

    var myOptionsInstance = Options.Create(options);
    services.AddSingleton(myOptionsInstance);

    services.AddStackExchangeRedisCache(redisCacheOptions =>
    {
      redisCacheOptions.ConfigurationOptions = options.RedisOptions.GetConfigurationOptions();
    });
    services.TryAddSingleton<IServerCache, ServerCacheRedisWithMemory>();
    
    services.AddACoreCacheModule(new ACoreCacheOptions
    {
      Categories = options.Categories,
      Expiration = options.MemoryCacheExpiration
    });
  }
}