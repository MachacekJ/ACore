using ACore.Models.Cache;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;
using ACore.Server.Repository;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.Mongo;

internal class SettingsDbModuleMongoRepositoryImpl : MongoContextBase, ISettingsDbModuleRepository
{
  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsPKMongoEntity));
  protected override string ModuleName => nameof(ISettingsDbModuleRepository);
  protected override IEnumerable<MongoVersionScriptsBase> AllUpdateVersions => Scripts.EFScriptRegistrations.AllVersions;

  private readonly IACoreServerCurrentScope _serverCurrentScope;
  private readonly IMongoCollection<SettingsPKMongoEntity> _settingsDbCollection;

  public SettingsDbModuleMongoRepositoryImpl(IACoreServerCurrentScope serverCurrentScope, IOptions<SettingsDbModuleOptions> options, IMediator mediator, ILogger<SettingsDbModuleMongoRepositoryImpl> logger)
    : base(serverCurrentScope, options.Value.MongoDb ?? throw new ArgumentNullException(nameof(options.Value.MongoDb)), mediator, logger)
  {
    _serverCurrentScope = serverCurrentScope;
    _settingsDbCollection = MongoDatabase.GetCollection<SettingsPKMongoEntity>(typeof(SettingsPKMongoEntity).GetCollectionName());
  }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;


  public async Task<RepositoryOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var findSetting = await GetSettingFromDbByKey(key);
    var setting = findSetting ?? new SettingsPKMongoEntity
    {
      Key = key,
      Value = value,
    };

    setting.Value = value;
    setting.IsSystem = isSystem;

    var res = await Save(_settingsDbCollection, setting);
    await _serverCurrentScope.ServerCache.Remove(CacheKeyTableSetting);
    return res;
  }

  private async Task<SettingsPKMongoEntity?> GetSettingFromDbByKey(string key)
  {
    using var cursor = await _settingsDbCollection.FindAsync(x => x.Key == key);
    var setting = await cursor.FirstOrDefaultAsync();
    return setting;
  }

  private async Task<SettingsPKMongoEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsPKMongoEntity>? allSettings;

    var allSettingsCacheResult = await _serverCurrentScope.ServerCache.Get<List<SettingsPKMongoEntity>>(CacheKeyTableSetting); //await _app.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCacheResult != null)
    {
      allSettings = allSettingsCacheResult;
    }
    else
    {
      var filter = Builders<SettingsPKMongoEntity>.Filter.Empty;
      using var cursor = await _settingsDbCollection.FindAsync(filter);
      allSettings = await cursor.ToListAsync();
      await _serverCurrentScope.ServerCache.Set(CacheKeyTableSetting, allSettings);
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(SettingsPKMongoEntity)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(SettingsPKMongoEntity)} table.");

    return vv;
  }

  #endregion
}