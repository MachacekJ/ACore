using ACore.Server.Configuration;
using ACore.Tests.Base;
using ACore.Tests.Server.TestImplementations.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure;

public abstract class ServerTestsBase : TestsBase
{
  protected virtual void SetupACoreTest(ACoreTestOptionsBuilder builder)
  {
  }

  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    services.AddACoreTest(SetupACoreTest);
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseACoreTest();
  }

  protected override void SetContainer(ContainerBuilder containerBuilder)
  {
    base.SetContainer(containerBuilder);
    containerBuilder.ContainerACoreServer();
    containerBuilder.ContainerACoreTest();
  }
}