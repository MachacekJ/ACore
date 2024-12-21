using ACore.Configuration.CQRS;
using ACore.Services.ACoreCache.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Configuration;

public static class ACoreServiceExtensions
{
  public static void AddACore(this IServiceCollection services, Action<ACoreOptionsBuilder>? options = null)
  {
    var opt = ACoreOptionsBuilder.Empty();
    options?.Invoke(opt);
    AddACore(services, opt.Build());
  }
  
  public static void AddACore(this IServiceCollection services, ACoreOptions opt)
  {
    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);
    services.AddACoreMediatr();
    services.AddACoreCacheModule(opt.ACoreCacheOptions);
  }

  public static void ContainerACore(this ContainerBuilder containerBuilder)
  {
   // containerBuilder.
  }
}