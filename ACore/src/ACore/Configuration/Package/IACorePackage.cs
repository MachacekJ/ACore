using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Configuration.Package;

public interface IACorePackage
{
  void RegisterServices(IServiceCollection services);
  Task UsePackage(IServiceProvider serviceProvider);
  void ConfigureContainer(ContainerBuilder containerBuilder);
}