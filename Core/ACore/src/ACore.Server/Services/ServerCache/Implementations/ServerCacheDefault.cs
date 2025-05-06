using ACore.Models.Cache;

namespace ACore.Server.Services.ServerCache.Implementations;

/// <summary>
/// No cache implementation.
/// </summary>
public class ServerCacheDefault : IServerCache
{
  public Task<TItem?> Get<TItem>(CacheKey key)
    => Task.FromResult<TItem?>(default);

  public Task Set<TItem>(CacheKey key, TItem value)
  {
    return Task.CompletedTask;
  }

  public Task Remove(CacheKey key)
  {
    return Task.CompletedTask;
  }

  public Task RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null)
  {
    return Task.CompletedTask;
  }
}