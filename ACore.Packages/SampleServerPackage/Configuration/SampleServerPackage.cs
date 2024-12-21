using ACore.Configuration.Package;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace SampleServerPackage.Configuration;

public class SampleServerPackage : IACorePackage
{
  public void RegisterServices(IServiceCollection services)
  {
   
  }

  public Task UsePackage(IServiceProvider serviceProvider)
  {
 return Task.CompletedTask;
  }

  public void ConfigureContainer(ContainerBuilder containerBuilder)
  {
   
  }
}