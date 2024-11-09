using ACore.Server.Modules.SettingsDbModule.Repositories.SQL.Models;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.SQL.Memory;

internal class SettingsDbModuleSqlMemoryRepositoryImplTestsSqlMemoryRepositoryImpl(DbContextOptions<SettingsDbModuleSqlMemoryRepositoryImplTestsSqlMemoryRepositoryImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlMemoryRepositoryImplTestsSqlMemoryRepositoryImpl> logger)
  : SettingsDbModuleSqlRepositoryImpl(options, mediator, logger)
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}