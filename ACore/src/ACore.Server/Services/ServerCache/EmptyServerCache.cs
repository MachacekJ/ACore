using ACore.Models.Cache;

namespace ACore.Server.Services.ServerCache;

public class EmptyServerCache : IServerCache
{
  public CacheCategory[] Categories { get; } = [];

  public Task<TItem?> Get<TItem>(CacheKey key)
    => Task.FromResult<TItem?>(default);

  public Task Set<TItem>(CacheKey key, TItem value)
  {
    return Task.CompletedTask;
  }

  public bool TryGetValue<TItem>(CacheKey key, out TItem? value)
  {
    value = default;
    return false;
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