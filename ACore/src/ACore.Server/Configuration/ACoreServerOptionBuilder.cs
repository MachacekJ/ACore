using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptionBuilder
{
  private readonly ACoreOptionsBuilder _aCoreOptionsBuilder = ACoreOptionsBuilder.Empty();
  private readonly SettingsDbModuleOptionsBuilder _settingsDbModuleOptionsBuilder = SettingsDbModuleOptionsBuilder.Empty();
  private readonly AuditModuleOptionsBuilder _auditModuleOptionsBuilder = AuditModuleOptionsBuilder.Empty();
  private readonly SecurityModuleOptionsBuilder _securityModuleOptionsBuilder = SecurityModuleOptionsBuilder.Empty();
  private ServerCacheOptions? _serverCacheOptions;
  public StorageOptionBuilder? DefaultStorageOptionBuilder;
  
  public static ACoreServerOptionBuilder Empty() => new();

  public ACoreServerOptionBuilder DefaultStorage(Action<StorageOptionBuilder> action)
  {
    DefaultStorageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(DefaultStorageOptionBuilder);
    return this;
  }

  public ACoreServerOptionBuilder AddSettingModule(Action<SettingsDbModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_settingsDbModuleOptionsBuilder);
    _settingsDbModuleOptionsBuilder.Activate();
    return this;
  }

  public ACoreServerOptionBuilder AddAuditModule(Action<AuditModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_auditModuleOptionsBuilder);
    _settingsDbModuleOptionsBuilder.Activate();
    _auditModuleOptionsBuilder.Activate();
    _securityModuleOptionsBuilder.Activate();
    return this;
  }

  public ACoreServerOptionBuilder AddServerCache(Action<ServerCacheOptions>? action = null)
  {
    _serverCacheOptions = new ServerCacheOptions(new StorageRedisOptions(string.Empty, string.Empty));
    action?.Invoke(_serverCacheOptions);
    return this;
  }

  public ACoreServerOptionBuilder AddSecurityModule(Action<SecurityModuleOptionsBuilder>? action)
  {
    action?.Invoke(_securityModuleOptionsBuilder);
    _securityModuleOptionsBuilder.Activate();
    return this;
  }

  // public ACoreServerOptionBuilder AddModule<T>(Action<T>? action = null) where T : ModuleOptionsBuilder
  // {
  //   var _builder = 
  //   action?.Invoke();
  // }

  public ACoreServerOptionBuilder ACore(Action<ACoreOptionsBuilder>? action = null)
  {
    action?.Invoke(_aCoreOptionsBuilder);
    return this;
  }

  public ACoreServerOptions Build()
  {
    return new ACoreServerOptions
    {
      DefaultStorages = DefaultStorageOptionBuilder?.Build(),
      ACoreOptions = _aCoreOptionsBuilder.Build(),
      SettingsDbModuleOptions = _settingsDbModuleOptionsBuilder.Build(DefaultStorageOptionBuilder),
      AuditModuleOptions = _auditModuleOptionsBuilder.Build(DefaultStorageOptionBuilder),
      SecurityModuleOptions = _securityModuleOptionsBuilder.Build(),
      ServerCache = _serverCacheOptions
    };
  }
}