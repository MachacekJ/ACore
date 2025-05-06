using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Repository.Configuration;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Services.Cache.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptions
{
  public string SaltForHash { get; set; } = string.Empty;
  public ACoreCacheOptions ACoreCacheOptions { get; set; } = new();

  public ServerRepositoryOptions? DefaultRepositories { get; set; }

  public ServerCacheOptions? ServerCache { get; set; }

  public SettingsDbModuleOptions? SettingsDbModuleOptions { get; set; }

  public AuditModuleOptions? AuditModuleOptions { get; set; }

  public SecurityModuleOptions? SecurityModuleOptions { get; set; }
  public LocalizationServerModuleOptions? LocalizationServerModuleOptions { get; set; }
}