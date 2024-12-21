using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(AuditModule), isActive);