using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Models.SaveInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACore.Server.Storages.Contexts.EF.Helpers;

public class SaveInfoHelper<TEntity, TPK>(IMediator mediator, IModel model, EFStorageDefinition storageDefinition, TEntity initData)
  where TEntity : PKEntity<TPK>
{
//  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), storageDefinition.Type)}_{nameof(IAuditStorageModule)}";
  private SaveInfoItem? _saveInfoItem;
  private IEntityType? _dbEntityType;

  public SaveInfoItem? SaveInfoItem => _saveInfoItem;

  public async Task Initialize()
  {
    ArgumentNullException.ThrowIfNull(initData.Id);
    // if (!await IsAuditEnabledAsync())
    //   return;

    _dbEntityType = model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var auditableAttribute = _dbEntityType.ClrType.IsAuditable();

    var tableName = GetTableName(_dbEntityType) ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = _dbEntityType.GetSchema();
    var userId = await GetUserId();
    _saveInfoItem = new SaveInfoItem(auditableAttribute != null, tableName, schemaName, auditableAttribute?.Version ?? 0, initData.Id, SaveInfoStateEnum.Unknown, userId);
    _saveInfoItem.SetPK(initData.Id);
  }

  public void InsertDbAction(TEntity savedData)
  {
    if (_saveInfoItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = savedData.Compare(null);
    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      _saveInfoItem.AddColumnEntry(new SaveInfoColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.FullName ?? d.Name, d.IsChange, d.RightValue, d.LeftValue));
    }

    _saveInfoItem.SetPK(savedData.Id);
    _saveInfoItem.SetEntityState(SaveInfoStateEnum.Added);
  }

  public void UpdateDbAction(TEntity oldData)
  {
    if (_saveInfoItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = oldData.Compare(initData);
    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      _saveInfoItem.AddColumnEntry(new SaveInfoColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.ACoreTypeName(), d.IsChange, d.LeftValue, d.RightValue));
    }


    _saveInfoItem.SetEntityState(SaveInfoStateEnum.Modified);
  }

  public void DeleteDbAction()
  {
    if (_saveInfoItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    var diff = initData.Compare(null);
    foreach (var d in diff)
    {
      var colName = GetColumnName<TEntity>(d.Name, _dbEntityType);
      _saveInfoItem.AddColumnEntry(new SaveInfoColumnItem(colName.IsAuditable, d.Name, colName.Name, d.Type.ACoreTypeName(), d.IsChange, d.LeftValue, d.RightValue));
    }

    _saveInfoItem.SetEntityState(SaveInfoStateEnum.Deleted);
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

  private async Task<string> GetUserId()
  {
    var user = await mediator.Send(new ICAMGetCurrentUserQuery());
    if (user.IsFailure)
      throw new Exception(user.ResultErrorItem.ToString());
    ArgumentNullException.ThrowIfNull(user.ResultValue);
    return user.ResultValue.ToString();
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