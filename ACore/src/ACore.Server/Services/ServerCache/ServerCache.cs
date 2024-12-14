using System.Text.Json;
using ACore.Configuration.Cache;
using ACore.Models.Cache;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Services.ACoreCache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;


namespace ACore.Server.Services.ServerCache;

public class ServerCache(IACoreCache aCoreCache, IDistributedCache distributedCache, IOptions<ServerCacheOptions> serverCacheOptions) : IServerCache
{
  public CacheCategory[] Categories { get; } = serverCacheOptions.Value.Categories.ToArray();

  public async Task<TItem?> Get<TItem>(CacheKey key)
  {
    var b = await distributedCache.GetStringAsync(GetKey(key));
    return b == null ? default : JsonSerializer.Deserialize<TItem>(b);
  }

  public async Task Set<TItem>(CacheKey key, TItem value)
  {
    await distributedCache.SetStringAsync(GetKey(key), JsonSerializer.Serialize(value));
  }

  public bool TryGetValue<TItem>(CacheKey key, out TItem? value)
  {
    throw new NotImplementedException();
  }

  public async Task Remove(CacheKey key)
  {
    await distributedCache.RemoveAsync(GetKey(key));
  }

  public async Task RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null)
  {
    var categoryKey = mainCategory.CategoryNameKey;
    var keyPrefix = subCategory?.CategoryNameKey;
    var cacheKeyPrefix = keyPrefix == null
      ? $"C:{categoryKey}^"
      : $"C:{categoryKey}^S:{keyPrefix}^";

    await using var m = await ConnectionMultiplexer.ConnectAsync(serverCacheOptions.Value.RedisOptions.GetConfigurationOptions());
    List<string> categories = [];

    foreach (var server in m.GetServers())
    {
      var keys = server.Keys(pattern: $"{cacheKeyPrefix}*").Select(a => a.ToString()).ToArray();
      categories.AddRange(keys);
    }

    foreach (var redisKey in categories.Distinct())
    {
      await distributedCache.RemoveAsync(redisKey);
    }
  }


  private string GetKey(CacheKey key)
  {
    if (Categories.All(k => k.CategoryNameKey != key.MainCategory.CategoryNameKey))
      throw new ArgumentException($"Cache - Category '{key.MainCategory.CategoryNameKey}' is not registered. Please register this category in {nameof(CacheOptionsBuilder.AddCacheCategories)}");

    return key.ToString();
  }
}