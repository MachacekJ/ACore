// using ACore.Server.Storages.Configuration;
// using SampleServerPackage.ToDoModulePG.Configuration;
//
// namespace SampleServerPackage.Configuration;
//
// public class SampleServerPackageOptionBuilder
// {
//   private readonly ToDoModuleOptionsBuilder _toDoModuleOptionsBuilder = ToDoModuleOptionsBuilder.Empty();
//
//   public StorageOptionBuilder? DefaultStorageOptionBuilder;
//
//   private StorageOptions? _storageOptions;
//   
//   private SampleServerPackageOptionBuilder()
//   {
//   }
//
//   public static SampleServerPackageOptionBuilder Empty() => new();
//
//   public SampleServerPackageOptionBuilder DefaultStorage(Action<StorageOptionBuilder> action)
//   {
//     DefaultStorageOptionBuilder ??= StorageOptionBuilder.Empty();
//     action(DefaultStorageOptionBuilder);
//     return this;
//   }
//
//   public SampleServerPackageOptionBuilder AddDefaultStorageOption(StorageOptions storageOptions)
//   {
//     _storageOptions = storageOptions;
//     return this;
//   }
//
//   public SampleServerPackageOptionBuilder AddToDoModule(Action<ToDoModuleOptionsBuilder>? action = null)
//   {
//     action?.Invoke(_toDoModuleOptionsBuilder);
//     _toDoModuleOptionsBuilder.Activate();
//     return this;
//   }
//   
//   public SampleServerPackageOptions Build()
//   {
//     return new SampleServerPackageOptions
//     {
//       DefaultStorages = DefaultStorageOptionBuilder?.Build(),
//       ToDoModuleOptions = _toDoModuleOptionsBuilder.Build(DefaultStorageOptionBuilder)
//     };
//   }
// }