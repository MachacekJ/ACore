using ACore.Modules.Base.Configuration;

namespace ACore.Server.Storages.Configuration;

public class StorageModuleOptions(string moduleName, bool isActive) : ModuleOptions(moduleName, isActive)
{
  public StorageOptions? Storages { get; init; }
}