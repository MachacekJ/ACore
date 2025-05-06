using ACore.Server.Modules.AuditModule.Repositories.EF.Models;
using ACore.Server.Modules.AuditModule.Repositories.EF.PG.Scripts;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.PG;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Repositories.EF.PG;

internal class AuditPGRepositoryImpl(DbContextOptions<AuditPGRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<AuditSqlRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => EFScriptRegistrations.AllVersions;
  protected override EFTypeDefinition EFTypeDefinition => new PGTypeDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditColumnEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditTableEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditUserEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditValueEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<AuditColumnEntity>(modelBuilder);
    SetDatabaseNames<AuditEntity>(modelBuilder);
    SetDatabaseNames<AuditTableEntity>(modelBuilder);
    SetDatabaseNames<AuditUserEntity>(modelBuilder);
    SetDatabaseNames<AuditValueEntity>(modelBuilder);
  }

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}