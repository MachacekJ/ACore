using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations.Configuration;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.SettingsDbModule;

public class SettingsDbModuleTestsBase() : StorageTestsBase([StorageTypeEnum.MemoryEF])
{
  protected ISettingsDbModuleRepository? MemorySettingStorageModule;

  protected override void SetupACoreTest(ACoreTestOptionsBuilder builder)
  {
    base.SetupACoreTest(builder);
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
    builder.AddSettingModule();
  }
  
  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    MemorySettingStorageModule = StorageResolver?.ReadFromStorage<ISettingsDbModuleRepository>(StorageTypeEnum.MemoryEF) ?? throw new ArgumentNullException($"{nameof(ISettingsDbModuleRepository)} is not implemented.");
  }
}