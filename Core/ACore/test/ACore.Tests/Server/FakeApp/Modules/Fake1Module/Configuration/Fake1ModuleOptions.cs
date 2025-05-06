using ACore.Server.Repository.Configuration;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;

public class Fake1ModuleOptions(ServerRepositoryOptions serverRepositories) : ServerRepositoryOptions(serverRepositories, nameof(Fake1Module));