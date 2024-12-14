using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Memory;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Repositories.EF.Memory;

internal class AuditSqlMemoryRepositoryImpl(DbContextOptions<AuditSqlMemoryRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<AuditSqlMemoryRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => [];
  protected override EFTypeDefinition EFTypeDefinition => new MemoryEFTypeDefinition();
}