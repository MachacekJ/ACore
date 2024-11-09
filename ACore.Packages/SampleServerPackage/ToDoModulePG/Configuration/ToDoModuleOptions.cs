using ACore.Server.Configuration.Modules;

namespace SampleServerPackage.ToDoModulePG.Configuration;

public class ToDoModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(ToDoModulePG), isActive);