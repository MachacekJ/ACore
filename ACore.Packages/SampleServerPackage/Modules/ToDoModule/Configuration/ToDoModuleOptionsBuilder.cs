using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Storages.Configuration;

namespace SampleServerPackage.ToDoModulePG.Configuration;

public class ToDoModuleOptionsBuilder: StorageModuleOptionBuilder
{
  public static ToDoModuleOptionsBuilder Empty() => new();


  public ToDoModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new ToDoModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbModuleRepository))
    };
  }
}