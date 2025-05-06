using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptions(ServerRepositoryOptions serverRepositories) : ServerRepositoryOptions(serverRepositories, nameof(SettingsDbModule));