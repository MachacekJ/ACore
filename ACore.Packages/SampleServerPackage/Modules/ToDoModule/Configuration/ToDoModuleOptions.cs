using ACore.Server.Storages.Configuration;

namespace SampleServerPackage.ToDoModulePG.Configuration;

public class ToDoModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(ToDoModulePG), isActive);