using ACore.Repository.Definitions.Models;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Tests.Server.FakeApp.Configuration;
using ACore.Tests.Server.TestInfrastructure;
using Microsoft.AspNetCore.Builder;

namespace ACore.Tests.Server.Tests.Modules.SettingsDbModule;

public class SettingsDbModuleTestsBase() : FakeAppTestsBase([RepositoryTypeEnum.MemoryEF])
{
  protected ISettingsDbModuleRepository? MemorySettingStorageModule;

  protected override void SetupBuilder(FakeAppOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
    builder.AddSettingModule();
    base.SetupBuilder(builder);
  }

  protected override async Task<IServiceProvider> UseServices(IApplicationBuilder appBuilder)
  {
    var sp = await base.UseServices(appBuilder);
    MemorySettingStorageModule = StorageResolver?.ReadRepositoryContext<ISettingsDbModuleRepository>(RepositoryTypeEnum.MemoryEF) ?? throw new ArgumentNullException($"{nameof(ISettingsDbModuleRepository)} is not implemented.");    
    return await Task.FromResult(sp);
  }
}