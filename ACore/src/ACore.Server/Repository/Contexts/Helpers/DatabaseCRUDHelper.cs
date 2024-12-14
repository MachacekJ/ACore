using ACore.Attributes;
using ACore.Extensions;
using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Contexts.Helpers.Models;
using ACore.Server.Repository.CQRS.Notifications;
using ACore.Server.Repository.Models.EntityEvent;
using ACore.Server.Repository.Results;
using ACore.Server.Repository.Results.Models;
using ACore.Server.Services;
using Mapster;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ACore.Server.Repository.Contexts.Helpers;

public class DatabaseCRUDHelper(IACoreServerCurrentScope serverCurrentScope, ILogger logger)
{

  /// <summary>
  /// Save entity. 
  /// --- Tasks --
  /// 1. Recognize is add or edit (Decision about add and edit depends on Empty value for each type.
  /// 2. Get current saved value and compare hashToCheck (All data types must be serializable)
  /// 
  /// </summary>
  public async Task<RepositoryOperationResult> Save<TEntity, TPK>(TEntity newData, DatabaseCRUDDefinitions<TEntity, TPK> crudDefinitions, DatabaseAuditDefinitions databaseAuditDefinitions,
    bool isNew, bool isInit, string? oldValueHash = null)
    where TEntity : PKEntity<TPK>
  {
    TEntity modifiedData;
    var hashIsRequired = newData.GetType().IsSumHashAllowed();
    var pkIdString = newData.Id?.ToString() ?? string.Empty;

    var auditableAttribute = typeof(TEntity).GetAuditableAttr();
    var entityEventOperationItem = new EntityEventItem(
      auditableAttribute != null,
      databaseAuditDefinitions.TableName,
      databaseAuditDefinitions.SchemaName,
      auditableAttribute?.Version ?? 0,
      newData.Id ?? throw new ArgumentNullException(nameof(modifiedData)),
      EntityEventEnum.Unknown,
      serverCurrentScope.CurrentUser.ToString());

    try
    {
      if (!isNew)
      {
        var oldData = await crudDefinitions.GetSavedDataFunc(newData.Id);
        if (oldData == null)
          return RepositoryOperationResult.ErrorEntityNotExists(typeof(TEntity).Name, pkIdString);

        modifiedData = oldData;

        //check db entity concurrency with hash
        if (hashIsRequired)
        {
          if (oldValueHash == null)
            throw new ArgumentNullException($"For update entity '{typeof(TEntity).Name}:{pkIdString}' is required a checkSum hash.");

          var saltForHash = GetSaltForHash();

          //Check consistency of entity.
          if (oldValueHash != modifiedData.GetSumHash(saltForHash))
            return RepositoryOperationResult.ErrorConcurrency(typeof(TEntity).Name, pkIdString);

          // Item has not been modified, save doesn't require.
          if (newData.GetSumHash(saltForHash) == oldValueHash)
            return RepositoryOperationResult.Success(RepositoryOperationTypeEnum.UnModified, oldValueHash);
        }

        UpdateEntityAction<TEntity, TPK>(databaseAuditDefinitions, entityEventOperationItem, oldData, newData);
        newData.Adapt(modifiedData);
        await crudDefinitions.UpdateDataFunc(modifiedData);
      }
      else
      {
        modifiedData = newData;
        modifiedData.Id = crudDefinitions.NewIdFunc();
        await crudDefinitions.AddDataFunc(modifiedData);
      }

      await crudDefinitions.SaveDataFunc();
      if (isNew)
        AddEntityAction<TEntity, TPK>(databaseAuditDefinitions, entityEventOperationItem, modifiedData);

      if (!isInit)
        await serverCurrentScope.Mediator.Publish(new RepositorySaveEventNotification(entityEventOperationItem));

      return RepositoryOperationResult.Success(
        isNew ? RepositoryOperationTypeEnum.Added : RepositoryOperationTypeEnum.Modified,
        hashIsRequired ? modifiedData.GetSumHash(GetSaltForHash()) : null);
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      return RepositoryOperationResult.InternalError(e);
    }
  }

  public async Task<RepositoryOperationResult> Delete<TEntity, TPK>(TPK id, DatabaseCRUDDefinitions<TEntity, TPK> crudDefinitions, DatabaseAuditDefinitions databaseAuditDefinitions, string? oldValueHash = null)
    where TEntity : PKEntity<TPK>
  {
    var pkIdString = id?.ToString() ?? string.Empty;
    var hashIsRequired = typeof(TEntity).IsSumHashAllowed();
    var oldData = await crudDefinitions.GetSavedDataFunc(id);
    if (oldData == null)
      return RepositoryOperationResult.ErrorEntityNotExists(typeof(TEntity).Name, pkIdString);

    var auditableAttribute = typeof(TEntity).GetAuditableAttr();
    var entityEventOperationItem = new EntityEventItem(
      auditableAttribute != null,
      databaseAuditDefinitions.TableName,
      databaseAuditDefinitions.SchemaName,
      auditableAttribute?.Version ?? 0,
      id ?? throw new ArgumentNullException(nameof(id)),
      EntityEventEnum.Unknown,
      serverCurrentScope.CurrentUser.ToString());

    try
    {
      //check db entity concurrency with hash
      if (hashIsRequired)
      {
        if (oldValueHash == null)
          throw new ArgumentNullException($"For delete entity '{typeof(TEntity).Name}:{pkIdString}' is required a checkSum hash.");

        var saltForHash = GetSaltForHash();

        //Check consistency of entity.
        if (oldValueHash != oldData.GetSumHash(saltForHash))
          return RepositoryOperationResult.ErrorConcurrency(typeof(TEntity).Name, pkIdString);
      }

      await crudDefinitions.DeleteDataFunc(id);
      await crudDefinitions.SaveDataFunc();

      DeleteEntityAction<TEntity, TPK>(databaseAuditDefinitions, entityEventOperationItem, oldData);
      await serverCurrentScope.Mediator.Publish(new RepositorySaveEventNotification(entityEventOperationItem));
      
      return RepositoryOperationResult.Success(
        RepositoryOperationTypeEnum.Deleted);
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      return RepositoryOperationResult.InternalError(e);
    }
  }

  public async Task<Version> GetVersion(string repositoryVersionKey, bool isInit)
  {
    if (!isInit)
      return new Version("0.0.0.0");

    var ver = await serverCurrentScope.Mediator.Send(new SettingsDbGetQuery(repositoryVersionKey));
    return ver is { IsSuccess: true, ResultValue: not null }
      ? new Version(ver.ResultValue)
      : new Version("0.0.0.0");
  }
  private string GetSaltForHash()
  {
    // Gets salt from global app settings.
    var saltForHash = serverCurrentScope.Options.SaltForHash;
    if (string.IsNullOrEmpty(saltForHash))
      logger.LogError($"Please configure salt for hash. Check application settings and paste hash string to section '{nameof(ACoreServerOptions)}.{nameof(ACoreServerOptions.SaltForHash)}'");

    return saltForHash;
  }

  private void UpdateEntityAction<TEntity, TPK>(DatabaseAuditDefinitions databaseAuditDefinitions, EntityEventItem entityEventItem, TEntity oldData, TEntity newData)
    where TEntity : PKEntity<TPK>
  {
    var diff = oldData.Compare(newData, (leftValue, rightValue) =>
    {
      if (rightValue is ObjectId enumRight && leftValue is ObjectId enumLeft)
        return !enumRight.Equals(enumLeft);
      if (databaseAuditDefinitions.IsMongoRounded && rightValue is DateTime enumRightDt && leftValue is DateTime enumLeftDt)
        return CompareMongoDateTime(enumLeftDt, enumRightDt) != 0;

      return null;
    });

    foreach (var d in diff)
    {
      var colName = databaseAuditDefinitions.ColumnAuditAttrInfo(d.Name);
      entityEventItem.AddColumnEntry(new EntityEventColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.ACoreTypeName(), d.IsChange, d.LeftValue, d.RightValue));
    }

    entityEventItem.SetEntityState(EntityEventEnum.Modified);
  }

  private void AddEntityAction<TEntity, TPK>(DatabaseAuditDefinitions databaseAuditDefinitions, EntityEventItem entityEventItem, TEntity savedData)
    where TEntity : PKEntity<TPK>
  {
    var allDifference = savedData.Compare(null);
    foreach (var difference in allDifference)
    {
      var colName = databaseAuditDefinitions.ColumnAuditAttrInfo(difference.Name);
      entityEventItem.AddColumnEntry(new EntityEventColumnItem(
        colName.IsAuditable,
        difference.Name,
        colName.Name,
        difference.Type.FullName ?? throw new Exception($"Unknown full name of type of entity {difference.Name}"),
        difference.IsChange,
        difference.RightValue,
        difference.LeftValue));
    }

    entityEventItem.SetPK(savedData.Id);
    entityEventItem.SetEntityState(EntityEventEnum.Added);
  }

  private void DeleteEntityAction<TEntity, TPK>(DatabaseAuditDefinitions databaseAuditDefinitions, EntityEventItem entityEventItem, TEntity savedData)
    where TEntity : PKEntity<TPK>
  {
    var diff = savedData.Compare(null);
    foreach (var difference in diff)
    {
      var colName = databaseAuditDefinitions.ColumnAuditAttrInfo(difference.Name);
      entityEventItem.AddColumnEntry(new EntityEventColumnItem(
        colName.IsAuditable,
        difference.Name,
        colName.Name,
        difference.Type.FullName ?? throw new Exception($"Unknown full name of type of entity {difference.Name}"),
        difference.IsChange,
        difference.LeftValue,
        difference.RightValue
      ));
    }

    entityEventItem.SetPK(savedData.Id);
    entityEventItem.SetEntityState(EntityEventEnum.Deleted);
  }

  public static int CompareMongoDateTime(DateTime a, DateTime b)
  {
    long ticksPerHundredth = 10000; // 0.0001s = 10 000 ticks
    long aTicks = (a.Ticks / ticksPerHundredth) * ticksPerHundredth;
    long bTicks = (b.Ticks / ticksPerHundredth) * ticksPerHundredth;

    return aTicks.CompareTo(bTicks);
  }
}