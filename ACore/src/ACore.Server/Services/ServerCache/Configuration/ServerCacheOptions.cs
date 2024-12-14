using ACore.Configuration.Cache;
using ACore.Server.Repository.Configuration.RepositoryTypes;

namespace ACore.Server.Services.ServerCache.Configuration;

public class ServerCacheOptions(RepositoryRedisOptions redisOptions): CacheOptions
{
  public RepositoryRedisOptions RedisOptions => redisOptions;
  public TimeSpan MemoryCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);
}