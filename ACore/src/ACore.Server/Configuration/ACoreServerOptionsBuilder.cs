using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptionsBuilder : ACoreOptionsBuilder
{
  //private readonly ACoreOptionsBuilder _aCoreOptionsBuilder = ACoreOptionsBuilder.Empty();
  private readonly SettingsDbModuleOptionsBuilder _settingsDbModuleOptionsBuilder = SettingsDbModuleOptionsBuilder.Empty();
  private readonly AuditModuleOptionsBuilder _auditModuleOptionsBuilder = AuditModuleOptionsBuilder.Empty();
  private readonly SecurityModuleOptionsBuilder _securityModuleOptionsBuilder = SecurityModuleOptionsBuilder.Empty();
  private ServerCacheOptions? _serverCacheOptions;
  public StorageOptionBuilder? DefaultStorageOptionBuilder;

  public static ACoreServerOptionsBuilder Empty() => new();

  public void DefaultStorage(Action<StorageOptionBuilder> action)
  {
    DefaultStorageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(DefaultStorageOptionBuilder);
  }

  public void AddSettingModule(Action<SettingsDbModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_settingsDbModuleOptionsBuilder);
    _settingsDbModuleOptionsBuilder.Activate();
  }

  public void AddAuditModule(Action<AuditModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_auditModuleOptionsBuilder);
    _settingsDbModuleOptionsBuilder.Activate();
    _auditModuleOptionsBuilder.Activate();
    _securityModuleOptionsBuilder.Activate();
  }

  public void AddServerCache(Action<ServerCacheOptions>? action = null)
  {
    _serverCacheOptions = new ServerCacheOptions(new StorageRedisOptions(string.Empty, string.Empty));
    action?.Invoke(_serverCacheOptions);
  }

  public ACoreServerOptionsBuilder AddSecurityModule(Action<SecurityModuleOptionsBuilder>? action)
  {
    action?.Invoke(_securityModuleOptionsBuilder);
    _securityModuleOptionsBuilder.Activate();
    return this;
  }

  public override ACoreServerOptions Build()
  {
    var res = new ACoreServerOptions();
    SetOptions(res);
    return res;
  }

  protected void SetOptions(ACoreServerOptions opt)
  {
    base.SetOptions(opt);
    SyncACoreCacheCategories();
    opt.DefaultStorages = DefaultStorageOptionBuilder?.Build();
    opt.SettingsDbModuleOptions = _settingsDbModuleOptionsBuilder.Build(DefaultStorageOptionBuilder);
    opt.AuditModuleOptions = _auditModuleOptionsBuilder.Build(DefaultStorageOptionBuilder);
    opt.SecurityModuleOptions = _securityModuleOptionsBuilder.Build();
    opt.ServerCache = _serverCacheOptions;
  }

  private void SyncACoreCacheCategories()
  {
    if (_serverCacheOptions?.Categories == null)
      return;

    foreach (var category in _serverCacheOptions.Categories)
    {
      CacheOptions.Categories.Add(category);
    }
  }
}