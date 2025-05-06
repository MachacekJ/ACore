using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions(ServerRepositoryOptions serverRepositories) : ServerRepositoryOptions(serverRepositories, nameof(AuditModule));