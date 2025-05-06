using ACore.Server.Modules.SettingsDbModule.Repositories.EF.Models;
using ACore.Server.Modules.SettingsDbModule.Repositories.EF.PG.Scripts;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.PG;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.EF.PG;

internal class SettingsEfModuleSqlPGRepositoryImpl(DbContextOptions<SettingsEfModuleSqlPGRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<SettingsEfModuleSqlPGRepositoryImpl> logger) 
  : SettingsEfModuleSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => EFScriptRegistrations.AllVersions;
  protected override EFTypeDefinition EFTypeDefinition => new PGTypeDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<SettingsEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}