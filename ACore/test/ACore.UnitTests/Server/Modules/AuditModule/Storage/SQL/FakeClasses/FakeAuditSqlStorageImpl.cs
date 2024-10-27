using ACore.Server.Modules.AuditModule.Storage.SQL;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.MemoryEFStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL.FakeClasses;

internal class FakeAuditSqlStorageImpl(DbContextOptions<AuditSqlStorageImpl> options, IMediator mediator, ILogger<AuditSqlMemoryStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
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