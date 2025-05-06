using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Repository.Configuration;
using ACore.Server.Repository.Configuration.RepositoryTypes;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Services.Cache.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptionsBuilder
{
  private readonly SettingsDbModuleOptionsBuilder _settingsDbModuleOptionsBuilder = SettingsDbModuleOptionsBuilder.Empty();
  private readonly AuditModuleOptionsBuilder _auditModuleOptionsBuilder = AuditModuleOptionsBuilder.Empty();
  private readonly SecurityModuleOptionsBuilder _securityModuleOptionsBuilder = SecurityModuleOptionsBuilder.Empty();
  private readonly LocalizationServerModuleOptionsBuilder _localizationServerModuleOptionsBuilder = LocalizationServerModuleOptionsBuilder.Empty();
  private readonly ACoreCacheOptions _cacheOptions = new();

  private ServerCacheOptions? _serverCacheOptions;
  protected readonly ServerRepositoryOptionBuilder DefaultRepositoryOptionBuilder = new();

  private string _saltForHash = string.Empty;

  public static ACoreServerOptionsBuilder Empty() => new();

  public void AddSaltForHash(string salt)
    => _saltForHash = salt;

  public void AddACoreCache(Action<ACoreCacheOptions> optionsAction)
    => optionsAction(_cacheOptions);

  public void AddDefaultRepositories(Action<ServerRepositoryOptionBuilder> action)
  {
    action(DefaultRepositoryOptionBuilder);
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
    _serverCacheOptions = new ServerCacheOptions(new RepositoryRedisOptions(string.Empty, string.Empty));
    action?.Invoke(_serverCacheOptions);
  }

  public ACoreServerOptionsBuilder AddLocalizationModule(string[] supportedLanguages, Action<LocalizationServerModuleOptionsBuilder>? action = null)
  {
    _localizationServerModuleOptionsBuilder.SetSupportedLanguages(supportedLanguages);
    action?.Invoke(_localizationServerModuleOptionsBuilder);
    _localizationServerModuleOptionsBuilder.Activate();
    _settingsDbModuleOptionsBuilder.Activate();
    return this;
  }

  public ACoreServerOptionsBuilder AddSecurityModule(Action<SecurityModuleOptionsBuilder>? action)
  {
    action?.Invoke(_securityModuleOptionsBuilder);
    _securityModuleOptionsBuilder.Activate();
    return this;
  }

  public virtual ACoreServerOptions Build()
  {
    var res = new ACoreServerOptions();
    SetOptions(res);
    return res;
  }

  protected void SetOptions(ACoreServerOptions opt)
  {
    SyncACoreCacheCategories();
    opt.SaltForHash = _saltForHash;
    opt.ACoreCacheOptions = _cacheOptions;
    opt.DefaultRepositories = DefaultRepositoryOptionBuilder.Build();
    opt.SettingsDbModuleOptions = _settingsDbModuleOptionsBuilder.IsActive ? _settingsDbModuleOptionsBuilder.Build(DefaultRepositoryOptionBuilder) : null;
    opt.AuditModuleOptions = _auditModuleOptionsBuilder.IsActive ? _auditModuleOptionsBuilder.Build(DefaultRepositoryOptionBuilder) : null;
    opt.SecurityModuleOptions = _securityModuleOptionsBuilder.IsActive ? _securityModuleOptionsBuilder.Build() : null;
    opt.LocalizationServerModuleOptions = _localizationServerModuleOptionsBuilder.IsActive ? _localizationServerModuleOptionsBuilder.Build(DefaultRepositoryOptionBuilder) : null;
    opt.ServerCache = _serverCacheOptions;
  }

  private void SyncACoreCacheCategories()
  {
    if (_serverCacheOptions?.Categories == null)
      return;

    foreach (var category in _serverCacheOptions.Categories)
    {
      _cacheOptions.Categories.Add(category);
    }
  }
}