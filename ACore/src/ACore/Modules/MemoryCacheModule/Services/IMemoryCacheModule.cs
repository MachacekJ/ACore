using ACore.Models.Cache;

namespace ACore.Modules.MemoryCacheModule.Services;

public interface IMemoryCacheModule
{
  CacheCategory[] Categories { get; }
  TItem? Get<TItem>(CacheKey key);
  void Set<TItem>(CacheKey key, TItem value);
  bool TryGetValue<TItem>(CacheKey key, out TItem? value);
  void Remove(CacheKey key);
  void RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null);
}