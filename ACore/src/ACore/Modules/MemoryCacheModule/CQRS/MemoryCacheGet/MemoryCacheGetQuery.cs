﻿using ACore.Models.Cache;
using ACore.Models.Result;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;

/// <summary>
/// Returns cache value for key.
/// null = cache value is not saved in cache.
/// <see cref="CacheValue"/>.<see cref="CacheValue.ObjectValue"/> = null, cache value is null, but is in cache.
/// </summary>
public class MemoryCacheModuleGetQuery(CacheKey key) : MemoryCacheModuleRequest<Result<CacheValue?>>
{
    public CacheKey Key { get; } = key;
}