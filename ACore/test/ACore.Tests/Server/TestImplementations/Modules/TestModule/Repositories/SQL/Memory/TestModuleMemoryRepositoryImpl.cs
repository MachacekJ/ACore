using ACore.Server.Services;
using ACore.Server.Storages.Definitions.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Memory;

internal class TestModuleMemoryRepositoryImpl(DbContextOptions<TestModuleMemoryRepositoryImpl> options, IACoreServerApp app, ILogger<TestModuleSqlRepositoryImpl> logger)
  : TestModuleSqlRepositoryImpl(options, app, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}