using ACore.Models.Cache;
using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;
using ACore.Server.Services.AppUser;
using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.Mongo;

internal class SettingsDbModuleMongoRepositoryImpl : DbContextBase, ISettingsDbModuleRepository
{
  private readonly IApp _app;

  public SettingsDbModuleMongoRepositoryImpl(DbContextOptions<SettingsDbModuleMongoRepositoryImpl> options, IApp app, ILogger<SettingsDbModuleMongoRepositoryImpl> logger)
    : base(options, app, logger)
  {
    _app = app;
    RegisterDbSet(Settings);
  }

  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsPKMongoEntity));

  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();
  protected override string ModuleName => nameof(ISettingsDbModuleRepository);

  public DbSet<SettingsPKMongoEntity> Settings { get; set; }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;


  public async Task<RepositoryOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var setting = await Settings.FirstOrDefaultAsync(i => i.Key == key);
    if (setting == null)
    {
      setting = new SettingsPKMongoEntity
      {
        Key = key
      };
      Settings.Add(setting);
    }

    setting.Value = value;
    setting.IsSystem = isSystem;

    var res = await Save<SettingsPKMongoEntity, ObjectId>(setting);

    _app.ServerCache.Remove(CacheKeyTableSetting);
   // await _mediator.Send(new MemoryCacheModuleRemoveKeyCommand(CacheKeyTableSetting));
    return res;
  }

  private async Task<SettingsPKMongoEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsPKMongoEntity>? allSettings;

    var allSettingsCacheResult = _app.ServerCache.Get<List<SettingsPKMongoEntity>>(CacheKeyTableSetting);  //await _app.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCacheResult != null)
    {
      // if (allSettingsCacheResult.ResultValue.ObjectValue == null)
      // {
      //   var ex = new Exception("The key '" + key + "' is not represented in settings table.");
      //   Logger.LogError("GetSettingsValue->" + key, ex);
      //   throw ex;
      // }

      allSettings = allSettingsCacheResult; //.ResultValue.ObjectValue as List<SettingsPKMongoEntity>;
    }
    else
    {
      allSettings = await Settings.ToListAsync();
      _app.ServerCache.Set(CacheKeyTableSetting, allSettings);
     // await _app.Send(new MemoryCacheModuleSaveCommand(CacheKeyTableSetting, allSettings));
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(Settings)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");

    return vv;
  }

  #endregion


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsPKMongoEntity>().ToCollection(CollectionNames.ObjectNameMapping[nameof(SettingsPKMongoEntity)].TableName);
    SetDatabaseNames<SettingsPKMongoEntity>(modelBuilder);
  }

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(CollectionNames.ObjectNameMapping, modelBuilder);
}