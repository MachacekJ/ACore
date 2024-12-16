using ACore.Configuration.Cache;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Services.ServerCache.Configuration;

public class ServerCacheOptions(StorageRedisOptions redisOptions): CacheOptions
{
  public StorageRedisOptions RedisOptions => redisOptions;
  public TimeSpan MemoryCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);
}