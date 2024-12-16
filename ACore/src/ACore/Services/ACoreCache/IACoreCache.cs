using ACore.Models.Cache;

namespace ACore.Services.ACoreCache;

public interface IACoreCache
{
  void Set<TItem>(CacheKey key, TItem value, TimeSpan? expiry = null);
  bool TryGetValue<TItem>(CacheKey key, out TItem? value);
  void Remove(CacheKey key);
  void RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null);
}