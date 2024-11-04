namespace ACore.Modules.Base.Configuration;

public interface IModuleOptions
{
  public string ModuleName { get; }
  public bool IsActive { get; }
  public IEnumerable<string>? Dependencies { get; }
}