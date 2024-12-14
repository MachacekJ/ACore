using ACore.Modules.MemoryCacheModule.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public static class MemoryCacheModuleServiceExtension
{
  public static void AddMemoryCacheModule(this IServiceCollection services, MemoryCacheModuleOptions options)
  {
    if (options.MemoryCacheOptionAction != null)
      services.AddMemoryCache(options.MemoryCacheOptionAction);
    else
      services.AddMemoryCache();
    
    //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCacheModulePipelineBehavior<,>));
    services.AddSingleton<IMemoryCacheModule, Services.MemoryCacheModule>();
  }
}