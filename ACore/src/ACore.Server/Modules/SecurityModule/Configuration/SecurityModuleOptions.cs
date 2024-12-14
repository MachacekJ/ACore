using ACore.Modules.Base.Configuration;

namespace ACore.Server.Modules.SecurityModule.Configuration;

public class SecurityModuleOptions : ModuleOptions
{
  public override string ModuleName => nameof(SecurityModule);
}