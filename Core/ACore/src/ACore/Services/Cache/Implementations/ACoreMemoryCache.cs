﻿using System.Collections;
using System.Reflection;
using ACore.Configuration.Cache;
using ACore.Models.Cache;
using ACore.Services.Cache.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ACore.Services.Cache.Implementations;

public class ACoreMemoryCache(IMemoryCache memoryCache, IOptions<ACoreCacheOptions> aCoreCacheOptions) : IACoreCache
{
  public CacheCategory[] Categories => aCoreCacheOptions.Value.Categories.ToArray();

  public TItem? Get<TItem>(CacheKey key)
  {
    return memoryCache.Get<TItem>(GetKey(key));
  }

  public void Set<TItem>(CacheKey key, TItem value, TimeSpan? expiry = null)
  {
    memoryCache.Set(GetKey(key), value, expiry ?? key.Duration ?? aCoreCacheOptions.Value.Expiration);
  }

  public bool TryGetValue<TItem>(CacheKey key, out TItem? value)
  {
    var res = memoryCache.TryGetValue(GetKey(key), out TItem? vall);
    value = vall;
    return res;
  }

  public void Remove(CacheKey key)
  {
    memoryCache.Remove(GetKey(key));
  }

  public void RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null)
  {
    var categoryKey = mainCategory.CategoryNameKey;
    var keyPrefix = subCategory?.CategoryNameKey;
    var cacheKeyPrefix = keyPrefix == null
      ? $"C:{categoryKey}^"
      : $"C:{categoryKey}^S:{keyPrefix}^";

    var keys = GetAllKeys(cacheKeyPrefix);
    foreach (var key in keys)
    {
      memoryCache.Remove(key);
    }
  }

  private string GetKey(CacheKey key)
  {
    if (Categories.All(k => k.CategoryNameKey != key.MainCategory.CategoryNameKey))
      throw new ArgumentException($"Cache - Category '{key.MainCategory.CategoryNameKey}' is not registered. Please register this category in {nameof(CacheOptions.Categories)}");

    return key.ToString();
  }

  /// <summary>
  /// For getting all keys the reflection is used, is not effective.
  /// </summary>
  private List<string> GetAllKeys(string? startWith = null)
  {
    var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

    var coherentStateValue = coherentState?.GetValue(memoryCache);

    var stringEntriesCollection = coherentStateValue?.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
    var stringEntriesCollectionValue = stringEntriesCollection?.GetValue(coherentStateValue) as ICollection;

    var keys = new List<string>();

    if (stringEntriesCollectionValue == null)
      return keys;

    foreach (var item in stringEntriesCollectionValue)
    {
      var methodInfo = item.GetType().GetProperty("Key");

      var val = methodInfo?.GetValue(item);
      if (val == null)
        continue;

      var stringKey = (string)val;
      if (startWith == null)
      {
        keys.Add(stringKey);
        continue;
      }

      if (stringKey.StartsWith(startWith))
        keys.Add(stringKey);
    }

    return keys;
  }
}