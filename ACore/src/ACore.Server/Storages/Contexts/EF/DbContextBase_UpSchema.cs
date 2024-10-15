using System.Reflection;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.Contexts.EF;

public abstract partial class DbContextBase
{
  private bool _isDatabaseInit = false;
 // private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(ISettingsDbModuleStorage)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  public async Task UpSchema()
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");

    // Get the latest implemented version, if any.
    _isDatabaseInit = await EFStorageDefinition.DatabaseIsInit(this, _options, mediator, Logger);
    if (!_isDatabaseInit)
    {
      var ver = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver is { IsSuccess: true, ResultValue: not null })
        lastVersion = new Version(ver.ResultValue);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (EFStorageDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
          updatedToVersion = await UpdateSchema(allVersions, lastVersion);
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
        updatedToVersion = await UpdateSchema(allVersions, lastVersion);
      }
    }

    if (this is ISettingsDbModuleStorage aa)
    {
      await aa.Setting_SaveAsync(StorageVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await mediator.Send(new SettingsDbSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateSchema(List<DbVersionScriptsBase> allVersions, Version lastVersion)
  {
    var updatedToVersion = new Version("0.0.0.0");

    foreach (var version in allVersions.Where(a => a.Version > lastVersion))
    {
      foreach (var script in version.AllScripts)
      {
        var idTrans = Guid.NewGuid();
        try
        {
          Logger.LogInformation("START {transaction} update script version:{version}", idTrans, version.Version);
          Logger.LogInformation("SCRIPT {transaction}:{script}", idTrans, script);
          await Database.ExecuteSqlRawAsync(script);
          Logger.LogInformation("END {transaction} update script version:{version}", idTrans, version.Version);
        }
        catch (Exception ex)
        {
          Logger.LogCritical(ex, "ERROR {transaction} update script version:{version}", idTrans, version.Version);
          throw;
        }
      }

      version.AfterScriptRunCode(this, _options, mediator, Logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }

  // private async Task<bool> DbIsEmpty()
  // {
  //    
  //   var res = true;
  //   
  //   try
  //   {
  //     var isSettingTable = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
  //     res = isSettingTable is { IsSuccess: true, ResultValue: null };
  //   }
  //   catch
  //   {
  //     // Log contains errors from EF.
  //     // 2024-10-12 07:46:42.368 +02:00 [ERR] An exception occurred while iterating over the results of a query for context type 'ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG.SettingsDbModuleSqlPGStorageImpl'.
  //     Logger.LogInformation("Setting table has not been found.");
  //   }
  //
  //   return res;
  // }
}