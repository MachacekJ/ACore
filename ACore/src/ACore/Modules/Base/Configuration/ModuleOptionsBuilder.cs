namespace ACore.Modules.Base.Configuration;

public class ModuleOptionsBuilder
{
  protected bool IsActive { get; private set; }

  public void Activate()
  {
    IsActive = true;
  }
}