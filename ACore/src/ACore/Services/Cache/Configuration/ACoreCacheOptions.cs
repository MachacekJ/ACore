using ACore.Configuration.Cache;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Services.Cache.Configuration;

public class ACoreCacheOptions : CacheOptions
{
  public Action<Microsoft.Extensions.Caching.Memory.MemoryCacheOptions>? MemoryCacheOptionAction { get; set; }
  public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
}