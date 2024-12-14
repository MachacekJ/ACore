using ACore.Server.Services;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;

internal class AuditSqlMemoryRepositoryImpl(DbContextOptions<AuditSqlMemoryRepositoryImpl> options, IACoreServerApp iaCoreServerApp, ILogger<AuditSqlMemoryRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, iaCoreServerApp, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}