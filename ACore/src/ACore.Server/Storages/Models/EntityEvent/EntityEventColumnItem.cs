namespace ACore.Server.Storages.Models.EntityEvent;

public record EntityEventColumnItem(bool IsAuditable, string PropName, string ColumnName, string DataType, bool IsChanged, object? OldValue, object? NewValue);

