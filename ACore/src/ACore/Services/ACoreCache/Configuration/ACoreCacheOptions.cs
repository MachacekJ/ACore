using ACore.Configuration.Cache;
using Microsoft.Extensions.Options;

namespace ACore.Services.ACoreCache.Configuration;

public class ACoreCacheOptions : CacheOptions, IOptions<ACoreCacheOptions>
{
  public Action<Microsoft.Extensions.Caching.Memory.MemoryCacheOptions>? MemoryCacheOptionAction { get; set; }
  public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
  public ACoreCacheOptions Value => this;
}