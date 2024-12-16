using ACore.Server.Services.ServerCache.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

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
      //   new ConfigurationOptions()
      // {
      //   Password = options.RedisOptions.Password,
      //   User = options.RedisOptions.UserName,
      //   ClientName = options.RedisOptions.InstanceName,
      //   EndPoints = { { options.RedisOptions.ConnectionString } }
      // };
      //redisCacheOptions.InstanceName = options.RedisOptions.InstanceName;
    });
    services.TryAddSingleton<IServerCache, ServerCacheRedisWithMemory>();
  }
}