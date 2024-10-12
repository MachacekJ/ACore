using ACore.Server.Configuration.Modules;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

public class TestModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(TestModule), isActive);