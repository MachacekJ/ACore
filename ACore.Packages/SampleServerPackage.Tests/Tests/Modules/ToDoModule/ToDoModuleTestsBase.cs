using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using SampleServerPackage.Configuration;

namespace SampleServerPackage.Tests.Tests.Modules.ToDoModule;

public class ToDoModuleTestsBase(IEnumerable<StorageTypeEnum> storages) : StorageTestsBase(storages)
{
  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    services.AddSampleServerModule(a => a.AddToDoModule());
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseSampleServerModule();
  }
}