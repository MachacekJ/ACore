using ACore.Server.Repository.Configuration;

namespace BlazorApp.Modules.CustomerModule.Configuration;

public class CustomerModuleOptions(ServerRepositoryOptions serverRepositories) : ServerRepositoryOptions(serverRepositories, nameof(CustomerModule));
