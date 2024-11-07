using ACore.Modules.Base.Configuration;

namespace ACore.Server.Modules.SecurityModule.Configuration;

public class SecurityModuleOptions(bool isActive = false) : ModuleOptions(nameof(SecurityModule), isActive)
{
  
}