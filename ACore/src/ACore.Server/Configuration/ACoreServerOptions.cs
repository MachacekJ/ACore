using ACore.Configuration;
using ACore.Configuration.Cache;
using ACore.Modules.Base.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptions
{
  public ACoreOptions ACoreOptions { get; init; } = new ();
  
  public StorageOptions? DefaultStorages { get; init; }
  
  public ServerCacheOptions? ServerCache { get; init; }
  
  public SettingsDbModuleOptions SettingsDbModuleOptions { get; init; } = new();

  public AuditModuleOptions AuditModuleOptions { get; init; } = new();

  public SecurityModuleOptions SecurityModuleOptions { get; init; } = new();
  
  public List<ModuleOptions> ExternalModulesOptions { get; init; } = new();
}