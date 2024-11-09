using ACore.Server.Storages.Configuration;
using SampleServerPackage.ToDoModulePG.Configuration;

namespace SampleServerPackage.Configuration;

public class SampleServerOptions
{
  public StorageOptions? DefaultStorages { get; init; }
  
  public ToDoModuleOptions ToDoModuleOptions { get; init; } = new();

}