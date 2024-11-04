using ACore.Modules.Base.Configuration;

namespace ACore.Server.Modules.ICAMModule.Configuration;

public class ICAMModuleOptions(bool isActive = false) : ModuleOptions(nameof(ICAMModule), isActive)
{
  
}