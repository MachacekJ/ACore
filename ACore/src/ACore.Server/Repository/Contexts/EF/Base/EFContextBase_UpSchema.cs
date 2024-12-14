using ACore.Repository.Definitions.Models;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Repository.Contexts.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Repository.Contexts.EF.Base;

public abstract partial class EFContextBase
{
  private bool _isDatabaseInFirstInit;

  private string RepositoryVersionKey => $"RepositoryVersion_{Enum.GetName(typeof(RepositoryTypeEnum), EFTypeDefinition.Type)}_{ModuleName}";

  public async Task UpSchema()
  {
    _isDatabaseInFirstInit = await EFTypeDefinition.DatabaseHasFirstUpdate(this, _options, _serverCurrentScope.Mediator, _logger);
    var lastVersion =  await _databaseCRUDHelper.GetVersion(RepositoryVersionKey, _isDatabaseInFirstInit);
    
    var updatedToVersion = lastVersion;

    if (AllUpdateVersions.Count() != 0)
    {
      if (EFTypeDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
          updatedToVersion = await UpdateSchema(AllUpdateVersions, lastVersion);
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          await transaction.RollbackAsync();
          throw new Exception("UpdateDbAsync is rollback", ex);
        }
      }
      else
      {
        updatedToVersion = await UpdateSchema(AllUpdateVersions, lastVersion);
      }
    }

    if (updatedToVersion <= lastVersion)
      return;

    if (this is ISettingsDbModuleRepository settingsDbModuleRepository)
    {
      await settingsDbModuleRepository.Setting_SaveAsync(RepositoryVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await _serverCurrentScope.Mediator.Send(new SettingsDbSaveCommand(EFTypeDefinition.Type, RepositoryVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateSchema(IEnumerable<EFVersionScriptsBase> allVersions, Version lastVersion)
  {
    var updatedToVersion = lastVersion;

    foreach (var version in allVersions.Where(a => a.Version > lastVersion))
    {
      foreach (var script in version.AllScripts)
      {
        var idTrans = Guid.NewGuid();
        try
        {
          _logger.LogInformation("START {transaction} update script version:{version}", idTrans, version.Version);
          _logger.LogInformation("SCRIPT {transaction}:{script}", idTrans, script);
          await Database.ExecuteSqlRawAsync(script);
          _logger.LogInformation("END {transaction} update script version:{version}", idTrans, version.Version);
        }
        catch (Exception ex)
        {
          _logger.LogCritical(ex, "ERROR {transaction} update script version:{version}", idTrans, version.Version);
          throw;
        }
      }

      version.AfterScriptRunCode(this, _options, _serverCurrentScope.Mediator, _logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }
}