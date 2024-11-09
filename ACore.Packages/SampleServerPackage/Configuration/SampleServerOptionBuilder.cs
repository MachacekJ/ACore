using ACore.Server.Storages.Configuration;
using SampleServerPackage.ToDoModulePG.Configuration;

namespace SampleServerPackage.Configuration;

public class SampleServerOptionBuilder
{
  private readonly ToDoModuleOptionsBuilder _toDoModuleOptionsBuilder = ToDoModuleOptionsBuilder.Empty();

  public StorageOptionBuilder? DefaultStorageOptionBuilder;

  private SampleServerOptionBuilder()
  {
  }

  public static SampleServerOptionBuilder Empty() => new();

  public SampleServerOptionBuilder DefaultStorage(Action<StorageOptionBuilder> action)
  {
    DefaultStorageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(DefaultStorageOptionBuilder);
    return this;
  }

  public SampleServerOptionBuilder AddToDoModule(Action<ToDoModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_toDoModuleOptionsBuilder);
    _toDoModuleOptionsBuilder.Activate();
    return this;
  }
  
  public SampleServerOptions Build()
  {
    return new SampleServerOptions
    {
      DefaultStorages = DefaultStorageOptionBuilder?.Build(),
      ToDoModuleOptions = _toDoModuleOptionsBuilder.Build(DefaultStorageOptionBuilder)
    };
  }
}