using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Services;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Models.EntityEvent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MongoDB.Bson;

namespace ACore.Server.Storages.Contexts.EF.Helpers;

public class EntityEventHelper<TEntity, TPK>(IACoreServerApp iIaCoreServerApp, IModel model, EFStorageDefinition storageDefinition, TEntity initData)
  where TEntity : PKEntity<TPK>
{
  private IEntityType? _dbEntityType;

  public EntityEventItem? EntityEventOperationItem { get; private set; }

  public async Task Initialize()
  {
    ArgumentNullException.ThrowIfNull(initData.Id);

    _dbEntityType = model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var auditableAttribute = _dbEntityType.ClrType.IsAuditable();

    var tableName = GetTableName(_dbEntityType) ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = _dbEntityType.GetSchema();
    var userId = await GetUserId();
    EntityEventOperationItem = new EntityEventItem(auditableAttribute != null, tableName, schemaName, auditableAttribute?.Version ?? 0, initData.Id, EntityEventEnum.Unknown, userId);
    EntityEventOperationItem.SetPK(initData.Id);
  }

  public void AddEntityAction(TEntity savedData)
  {
    if (EntityEventOperationItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = savedData.Compare(null);
    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      EntityEventOperationItem.AddColumnEntry(new EntityEventColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.FullName ?? d.Name, d.IsChange, d.RightValue, d.LeftValue));
    }

    EntityEventOperationItem.SetPK(savedData.Id);
    EntityEventOperationItem.SetEntityState(EntityEventEnum.Added);
  }

  public void UpdateEntityAction(TEntity oldData)
  {
    if (EntityEventOperationItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = oldData.Compare(initData, (leftValue, rightValue) =>
    {
      if (rightValue is ObjectId enumRight && leftValue is ObjectId enumLeft)
        return !enumRight.Equals(enumLeft);
      return null;
    });

    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      EntityEventOperationItem.AddColumnEntry(new EntityEventColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.ACoreTypeName(), d.IsChange, d.LeftValue, d.RightValue));
    }


    EntityEventOperationItem.SetEntityState(EntityEventEnum.Modified);
  }

  public void DeleteEntityAction()
  {
    if (EntityEventOperationItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = initData.Compare(null);
    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      EntityEventOperationItem.AddColumnEntry(new EntityEventColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.ACoreTypeName(), d.IsChange, d.LeftValue, d.RightValue));
    }

    EntityEventOperationItem.SetEntityState(EntityEventEnum.Deleted);
  }

  private string? GetTableName(IReadOnlyEntityType dbEntityType)
  {
    var tableName = dbEntityType.GetTableName();
    if (string.IsNullOrEmpty(storageDefinition.DataAnnotationTableNameKey))
      return tableName;

    var anno = dbEntityType.GetAnnotation(storageDefinition.DataAnnotationTableNameKey).Value?.ToString();
    if (anno != null)
      tableName = anno;

    return tableName;
  }

  private Task<string> GetUserId()
  {
    return Task.FromResult(iIaCoreServerApp.CurrentUser.ToString());
  }

  private (string Name, bool IsAuditable) GetColumnName<T>(string propName, IEntityType dbEntityType)
  {
    if (propName.StartsWith('.'))
      propName = propName.Substring(1);

    var isAuditable = false;
    var columnName = string.Empty;

    var auditableAttribute = typeof(T).IsAuditable(propName); //auditConfiguration?.NotAuditProperty
    if (auditableAttribute != null)
      isAuditable = true;

    var property = dbEntityType.GetProperties().SingleOrDefault(property => property.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
    if (property == null)
      return (columnName, isAuditable);

    columnName = property.GetColumnName();
    if (string.IsNullOrEmpty(storageDefinition.DataAnnotationColumnNameKey))
      return (columnName, isAuditable);

    var anno = property.GetAnnotations().FirstOrDefault(e => e.Name == storageDefinition.DataAnnotationColumnNameKey);
    if (anno != null)
      columnName = anno.Value?.ToString() ?? throw new Exception($"Annotation '{storageDefinition.DataAnnotationColumnNameKey}' has not been found for property '{propName}'");

    return (columnName, isAuditable);
  }
}