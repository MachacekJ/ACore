using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Storages.Configuration;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

public class TestModuleOptionsBuilder: StorageModuleOptionBuilder
{
  public static TestModuleOptionsBuilder Empty() => new();


  public TestModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new TestModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbModuleRepository))
    };
  }
}