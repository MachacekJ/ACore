using ACore.Server.Modules.AuditModule.Repositories.SQL;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;
using ACore.Server.Services;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL.FakeClasses;

internal class FakeAuditSqlRepositoryImpl(DbContextOptions<AuditSqlRepositoryImpl> options, IACoreServerApp iaCoreServerApp, ILogger<AuditSqlMemoryRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, iaCoreServerApp, logger)
{
  protected override EFStorageDefinition EFStorageDefinition { get; } = new MemoryEFStorageDefinition();
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
  }
}