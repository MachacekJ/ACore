using ACore.Server.Modules.SettingsDbModule.Repositories.SQL.Models;
using ACore.Server.Services;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.SQL.Memory;

internal class SettingsDbModuleRepositoryImpl(DbContextOptions<SettingsDbModuleRepositoryImpl> options, IACoreServerApp iaCoreServerApp, ILogger<SettingsDbModuleRepositoryImpl> logger)
  : SettingsDbModuleSqlRepositoryImpl(options, iaCoreServerApp, logger)
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}