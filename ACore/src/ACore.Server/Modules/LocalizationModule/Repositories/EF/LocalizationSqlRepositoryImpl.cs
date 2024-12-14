using ACore.Models.Cache;
using ACore.Modules.LocalizationModule.Repositories;
using ACore.Server.Modules.LocalizationModule.Repositories.EF.Models;
using ACore.Server.Repository;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Services;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.LocalizationModule.Repositories.EF;

internal abstract class LocalizationSqlRepositoryImpl(DbContextOptions options, IACoreServerCurrentScope serverCurrentScope, ILogger<LocalizationSqlRepositoryImpl> logger)
  : EFContextBase(options, serverCurrentScope, logger), ILocalizationRepository, IDbRepository
{
  private readonly IACoreServerCurrentScope _serverCurrentScope = serverCurrentScope;
  internal static CacheKey LocalizationCacheKey(string contextId, int lcid) => CacheKey.Create(CacheCategories.Localization, new CacheCategory(lcid.ToString()), contextId);

  protected override string ModuleName => nameof(ILocalizationRepository);
  protected override List<EFVersionScriptsBase> AllUpdateVersions => PG.Scripts.EFScriptRegistrations.AllVersions;


  // ReSharper disable once UnusedAutoPropertyAccessor.Global
  public DbSet<ACoreLocalizationEntity> Localizations { get; set; }

  public IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey,  int lcid)
  {
    var dic = GetLocalizationRecords(localizationKey.ContextId.Name, lcid);
    return dic.GetValueOrDefault(localizationKey.Key);
  }

  public IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<IACoreLocalizationItem> GetAllRecords(string contextId, int lcid)
    => GetLocalizationRecords(contextId, lcid)
      .Select(a => a.Value)
      .ToList<IACoreLocalizationItem>();

  // TODO blokujici !!!!!!!! Opravit neni async !!!!!
  private Dictionary<string, ACoreLocalizationItem> GetLocalizationRecords(string contextId, int lcid)
  {
    var cacheKey = LocalizationCacheKey(contextId, lcid);
    // get from cache
    var cacheValue = _serverCurrentScope.ServerCache.Get<Dictionary<string, ACoreLocalizationItem>>(cacheKey).Result;
    if (cacheValue != null)
      return cacheValue;

    // get from repository and save to cache
    var localizationRecords = Localizations
      .Where(a => a.ContextId == contextId && a.Lcid == lcid)
      .Select(a => a.ToLocalizationRecord())
      .ToDictionary(k => k.Key, v => v);
    _serverCurrentScope.ServerCache.Set(cacheKey, localizationRecords).Wait();
    return localizationRecords;
  }
}