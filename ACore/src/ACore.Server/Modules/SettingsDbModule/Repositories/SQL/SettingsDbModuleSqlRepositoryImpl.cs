﻿using ACore.Models.Cache;
using ACore.Server.Modules.SettingsDbModule.Repositories.SQL.Models;
using ACore.Server.Services;
using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.SQL;

internal abstract class SettingsDbModuleSqlRepositoryImpl : DbContextBase, ISettingsDbModuleRepository
{
  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsEntity));
  private readonly IACoreServerApp _iaCoreServerApp;
  protected override string ModuleName => nameof(ISettingsDbModuleRepository);

  public DbSet<SettingsEntity> Settings { get; set; }

  protected SettingsDbModuleSqlRepositoryImpl(DbContextOptions options, IACoreServerApp iaCoreServerApp, ILogger<SettingsDbModuleSqlRepositoryImpl> logger)
    : base(options, iaCoreServerApp, logger)
  {
    _iaCoreServerApp = iaCoreServerApp;
    RegisterDbSet(Settings);
  }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;

  public async Task<RepositoryOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var set = await Settings.FirstOrDefaultAsync(i => i.Key == key)
              ?? new SettingsEntity
              {
                Key = key
              };

    set.Value = value;
    set.IsSystem = isSystem;

    var res = await Save<SettingsEntity, int>(set);

    await _iaCoreServerApp.ServerCache.Remove(CacheKeyTableSetting);
    //await _mediator.Send(new MemoryCacheModuleRemoveKeyCommand(CacheKeyTableSetting));
    return res;
  }

  private async Task<SettingsEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsEntity>? allSettings;

    var allSettingsCacheResult = await _iaCoreServerApp.ServerCache.Get<List<SettingsEntity>>(CacheKeyTableSetting); //await _mediator.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCacheResult != null)
    {
      // if (allSettingsCacheResult.ResultValue.ObjectValue == null)
      // {
      //   var ex = new Exception("The key '" + key + "' is not represented in settings table.");
      //   Logger.LogError("GetSettingsValue->" + key, ex);
      //   throw ex;
      // }

      allSettings = allSettingsCacheResult; // as List<SettingsEntity>;
    }
    else
    {
      allSettings = await Settings.ToListAsync();
      _iaCoreServerApp.ServerCache.Set(CacheKeyTableSetting, allSettings);
      //await _mediator.Send(new MemoryCacheModuleSaveCommand(CacheKeyTableSetting, allSettings));
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(Settings)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");

    return vv;
  }

  #endregion
}