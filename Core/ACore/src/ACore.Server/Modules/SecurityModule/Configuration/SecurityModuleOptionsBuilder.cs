using ACore.Modules.Base.Configuration;

namespace ACore.Server.Modules.SecurityModule.Configuration;

public class SecurityModuleOptionsBuilder: ModuleOptionsBuilder
{
  public static SecurityModuleOptionsBuilder Empty() => new();
  public SecurityModuleOptions Build()
  {
    var res = new SecurityModuleOptions();
    SetBase(res);
    return res;
  }
}