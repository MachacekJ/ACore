using ACore.Server.Configuration;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestInfrastructure;
using ACore.Tests.Server.TestInfrastructure.Storages.EF;
using Microsoft.Extensions.DependencyInjection;
using SampleServerPackage.Configuration;

namespace SampleServerPackage.Tests.Tests.Modules.ToDoModule;

public class ToDoModuleTestsBase(IEnumerable<StorageTypeEnum> storages) : StorageTestsBase(storages)
{
  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    
    // services.AddSampleServerPackage(a =>
    // {
    //   a.AddToDoModule();
    // });
  }

  // protected override void SetupACoreServer(ACoreServerOptionsBuilder builder)
  // {
  //   base.SetupACoreServer(builder);
  //   //builder.AddModule()
  // }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    // await sp.UseACoreServer();
    // await sp.UseACoreSampleServerPackage();
  }
}