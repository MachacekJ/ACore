using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestInfrastructure;

namespace ACore.Tests.Server.Tests.Modules.SettingsDbModule;

public class SettingsDbModuleTestsBase() : StorageTestsBase(StorageTypeEnum.MemoryEF)
{
  protected ISettingsDbModuleStorage? MemorySettingStorageModule;

  protected override void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    base.SetupACoreServer(builder);
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
  }
  
  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    MemorySettingStorageModule = StorageResolver?.ReadFromStorage<ISettingsDbModuleStorage>(StorageTypeEnum.MemoryEF) ?? throw new ArgumentNullException($"{nameof(ISettingsDbModuleStorage)} is not implemented.");
  }
}