using ACore.Configuration.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public class MemoryCacheModuleOptionsBuilder : CacheOptionsBuilder
{
  private Action<MemoryCacheOptions>? _memoryCacheOptionsAction;
  
  public static MemoryCacheModuleOptionsBuilder Empty() => new();

  private MemoryCacheModuleOptionsBuilder()
  {
  }

  public MemoryCacheModuleOptions Build()
  {
    var baseBuild = BuildBase();
    return new MemoryCacheModuleOptions(IsActive)
    {
      MemoryCacheOptionAction = _memoryCacheOptionsAction,
      Categories = baseBuild.Categories
    };
  }
}