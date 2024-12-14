using ACore.Server.Modules.SettingsDbModule.Repositories.EF.Models;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Memory;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.EF.Memory;

internal class SettingsEfModuleRepositoryImpl(DbContextOptions<SettingsEfModuleRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<SettingsEfModuleRepositoryImpl> logger)
  : SettingsEfModuleSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => [];
  protected override EFTypeDefinition EFTypeDefinition => new MemoryEFTypeDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}