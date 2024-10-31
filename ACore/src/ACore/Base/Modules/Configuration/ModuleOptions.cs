namespace ACore.Base.Modules.Configuration;

public abstract class ModuleOptions(string moduleName, bool isActive, IEnumerable<string>? dependencies = null) : IModuleOptions
{
  public string ModuleName => moduleName;
  public bool IsActive => isActive;
  public IEnumerable<string>? Dependencies => dependencies;
}