namespace ACore.Base.Modules.Configuration;

public interface IModuleOptions
{
  public string ModuleName { get; }
  public bool IsActive { get; }
  public IEnumerable<string>? Dependencies { get; }
}