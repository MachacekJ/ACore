﻿using ACore.Models.Cache;
using ACore.Services.ACoreCache;

namespace ACore.Server.Services.ServerCache;

public interface IServerCache
{
  Task Set<TItem>(CacheKey key, TItem value);
  Task<TItem?> Get<TItem>(CacheKey key);
  
  Task Remove(CacheKey key);
  Task RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory = null);
}