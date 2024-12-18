using ACore.Configuration.Cache;
using ACore.Modules.Base.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public class MemoryCacheModuleOptions(bool isActive) : CacheOptions, IModuleOptions, IOptions<MemoryCacheModuleOptions>
{
  public Action<MemoryCacheOptions>? MemoryCacheOptionAction { get; init; }
  public MemoryCacheModuleOptions Value => this;

  public string ModuleName => nameof(MemoryCacheModule);
  public bool IsActive => isActive;
  public IEnumerable<string>? Dependencies => null;
}