using ACore.Configuration;
using ACore.Modules.MemoryCacheModule.Repositories;
using ACore.Server.Configuration;
using ACore.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Core.Modules.MemoryCacheModule
{
  public class MemoryCacheModuleBaseTests(bool onlyCacheTest = false) : TestsBase
  {
    protected IMemoryCacheModuleRepository? MemoryCacheStorage { get; private set; }

    protected override void RegisterServices(ServiceCollection sc)
    {
      base.RegisterServices(sc);
      sc.AddACore(o =>
        //o.AddSaltForHash("test")
          o.AddMemoryCacheModule(op =>
            op.AddCacheCategories(
              CacheTestCategories.CacheTest,
              CacheTestCategories.CacheTest2
            )
          ));
    }

    protected override async Task GetServices(IServiceProvider sp)
    {
      await base.GetServices(sp);
      await sp.UseACoreServer();
      MemoryCacheStorage = sp.GetService<IMemoryCacheModuleRepository>() ?? throw new ArgumentException($"{nameof(IMemoryCacheModuleRepository)} is null.");
    }
  }
}