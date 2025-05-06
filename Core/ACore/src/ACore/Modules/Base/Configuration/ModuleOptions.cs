namespace ACore.Modules.Base.Configuration;

public abstract class ModuleOptions : IModuleOptions
{
  public abstract string ModuleName { get; }
  public bool IsActive { get; set; }
}