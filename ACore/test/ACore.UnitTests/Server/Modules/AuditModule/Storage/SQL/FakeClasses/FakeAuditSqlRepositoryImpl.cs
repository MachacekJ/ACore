using ACore.Server.Modules.AuditModule.Repositories.EF;
using ACore.Server.Modules.AuditModule.Repositories.EF.Memory;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Memory;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Server.Modules.AuditModule.Storage.SQL.FakeClasses;

internal class FakeAuditSqlRepositoryImpl(DbContextOptions<AuditSqlRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<AuditSqlMemoryRepositoryImpl> logger)
  : AuditSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => [];
  protected override EFTypeDefinition EFTypeDefinition { get; } = new MemoryEFTypeDefinition();
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
  }
}