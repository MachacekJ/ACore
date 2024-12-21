using ACore.Modules.Base.Configuration;

namespace ACore.Server.Storages.Configuration;

public class StorageModuleOptionBuilder : ModuleOptionsBuilder
{
  private StorageOptionBuilder? _storageOptionBuilder;
  protected void StoragesBase(Action<StorageOptionBuilder> action)
  {
    _storageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(_storageOptionBuilder);
  }

  protected StorageOptions BuildStorage(StorageOptionBuilder? defaultStorages, string moduleName)
  {
    if (defaultStorages == null)
      return _storageOptionBuilder?.Build() ?? StorageOptionBuilder.Empty().Build();
    
    _storageOptionBuilder ??= defaultStorages;

    return _storageOptionBuilder.Build();
  }
}