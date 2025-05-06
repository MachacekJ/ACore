namespace ACore.Modules.Base.Configuration;

public class ModuleOptionsBuilder
{
  public bool IsActive { get; private set; }

  public void Activate()
  {
    IsActive = true;
  }

  protected virtual void SetBase(ModuleOptions moduleOptions)
  {
    moduleOptions.IsActive = IsActive;
  }
}