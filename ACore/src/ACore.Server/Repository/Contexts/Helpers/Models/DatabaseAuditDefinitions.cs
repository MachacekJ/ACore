namespace ACore.Server.Repository.Contexts.Helpers.Models;

public record DatabaseAuditDefinitions(string TableName, string? SchemaName, bool IsMongoRounded, Func<string, (string Name, bool IsAuditable)> ColumnAuditAttrInfo);