using System.Text.Json;
using ACore.Configuration.Cache;
using ACore.Models.Cache;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Services.ServerCache.CQRS.Notification;
using ACore.Server.Services.ServerCache.Models;
using ACore.Services.ACoreCache;
using ACore.Services.ACoreCache.Configuration;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ACore.Server.Services.ServerCache.Implementations;

/// <summary>
/// Cache combines Redis and Memory cache
/// Calls notification <see cref="ServerCacheAddItemNotification"/>.
/// </summary>
public class ServerCacheRedisWithMemory(IACoreCache aCoreCache, IDistributedCache distributedCache, IMediator mediator, IOptions<ACoreCacheOptions> coreCache, IOptions<ServerCacheOptions> serverCacheOptions) : IServerCache
{
  private CacheCategory[] Categories { get; } = serverCacheOptions.Value.Categories.ToArray();

  public async Task Set<TItem>(CacheKey key, TItem value)
  {
    var memDuration = key.Duration ?? coreCache.Value.Expiration;
    aCoreCache.Set(key, value);
    await mediator.Publish(new ServerCacheAddItemNotification(key, value, ServerCacheTypeEnum.Memory, memDuration));

    var duration = key.Duration ?? serverCacheOptions.Value.RedisOptions.Expiration;
    await distributedCache.SetStringAsync(GetKey(key), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions()
    {
      AbsoluteExpirationRelativeToNow = duration
    });
    await mediator.Publish(new ServerCacheAddItemNotification(key, value, ServerCacheTypeEnum.Redis, duration));
  }

  public async Task<TItem?> Get<TItem>(CacheKey key)
  {
    var aCoreCacheResult = aCoreCache.TryGetValue(key, out TItem? value);
    if (aCoreCacheResult)
    {
      await mediator.Publish(new ServerCacheGetItemNotification(key, ServerCacheTypeEnum.Memory, true));
      return value;
    }

    await mediator.Publish(new ServerCacheGetItemNotification(key, ServerCacheTypeEnum.Memory, false));

    var distributedCacheResult = await distributedCache.GetStringAsync(GetKey(key));
    if (distributedCacheResult != null)
    {
      await mediator.Publish(new ServerCacheGetItemNotification(key, ServerCacheTypeEnum.Redis, true));
      return JsonSerializer.Deserialize<TItem>(distributedCacheResult);
    }

    await mediator.Publish(new ServerCacheGetItemNotification(key, ServerCacheTypeEnum.Redis, false));
    return default;
  }

  public async Task Remove(CacheKey key)
  {
    await distributedCache.RemoveAsync(GetKey(key));
    aCoreCache.Remove(key);
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

    aCoreCache.RemoveCategory(mainCategory, subCategory);
  }


  private string GetKey(CacheKey key)
  {
    if (Categories.All(k => k.CategoryNameKey != key.MainCategory.CategoryNameKey))
      throw new ArgumentException($"Cache - Category '{key.MainCategory.CategoryNameKey}' is not registered. Please register this category in {nameof(CacheOptions.Categories)}");

    return key.ToString();
  }
}