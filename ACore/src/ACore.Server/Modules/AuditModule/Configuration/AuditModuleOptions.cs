using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(AuditModule), isActive)
{
}