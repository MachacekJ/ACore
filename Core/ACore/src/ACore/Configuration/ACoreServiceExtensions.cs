using ACore.Configuration.CQRS;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Configuration;

public static class ACoreServiceExtensions
{
 
  public static void AddACore(this IServiceCollection services)
  {
    services.AddACoreMediatr();
  }

  public static void ContainerACore(this ContainerBuilder containerBuilder)
  {

  }
}