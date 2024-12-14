using ACore.Server.Modules.AuditModule.Repositories.SQL.Models;
using ACore.Server.Services.AppUser;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Repositories.SQL.PG;

internal class AuditPGEFRepositoryImpl(DbContextOptions<AuditPGEFRepositoryImpl> options, IApp app, ILogger<AuditSqlRepositoryImpl> logger) : AuditSqlRepositoryImpl(options, app, logger)
{
  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new PGStorageDefinition();
  
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

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(AuditPGEFDbNames.ObjectNameMapping, modelBuilder);
}