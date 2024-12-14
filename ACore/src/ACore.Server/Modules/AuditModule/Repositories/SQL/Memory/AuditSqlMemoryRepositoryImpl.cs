using ACore.Server.Services.AppUser;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;

internal class AuditSqlMemoryRepositoryImpl(DbContextOptions<AuditSqlMemoryRepositoryImpl> options, IApp app, ILogger<AuditSqlMemoryRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, app, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}