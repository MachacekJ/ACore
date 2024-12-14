namespace ACore.Server.Repository.Models.EntityEvent;

public class EntityEventItem
{
  public bool IsAuditable { get; set; }
  public string TableName { get; }
  public string? SchemaName { get; }
  public int Version { get; }
  public List<EntityEventColumnItem> ChangedColumns { get; set; } = [];
  public EntityEventEnum EntityState { get; private set; }
  public string UserId { get; private set; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }

  public EntityEventItem(bool isAuditable, string tableName, string? schemaName, int version, object pkValue, EntityEventEnum entityState, string userId)
  {
    IsAuditable = isAuditable;
    TableName = tableName;
    SchemaName = schemaName;
    EntityState = entityState;
    Version = version;
    UserId = userId;
    SetPK(pkValue);
  }

  public void AddColumnEntry(EntityEventColumnItem columnItem)
  => ChangedColumns.Add(columnItem);
  

  public void SetEntityState(EntityEventEnum entityState)
  {
    EntityState = entityState;
  }

  public void SetPK<TPK>(TPK pkValue)
  {
    ArgumentNullException.ThrowIfNull(pkValue);

    if (long.TryParse(pkValue.ToString(), out var pkv))
      PkValue = pkv;
    else
      PkValueString = pkValue.ToString();
  }
}