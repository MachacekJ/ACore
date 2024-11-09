using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Memory;

internal class TestModuleMemoryRepositoryImpl(DbContextOptions<TestModuleMemoryRepositoryImpl> options, IMediator mediator, ILogger<TestModuleSqlRepositoryImpl> logger)
  : TestModuleSqlRepositoryImpl(options, mediator, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}