using ACore.Configuration;
using ACore.Configuration.Cache;
using ACore.Modules.Base.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptions : ACoreOptions
{
  public StorageOptions? DefaultStorages { get; set; }
  
  public ServerCacheOptions? ServerCache { get; set; }
  
  public SettingsDbModuleOptions SettingsDbModuleOptions { get; set; } = new();

  public AuditModuleOptions AuditModuleOptions { get; set; } = new();

  public SecurityModuleOptions SecurityModuleOptions { get; set; } = new();
}