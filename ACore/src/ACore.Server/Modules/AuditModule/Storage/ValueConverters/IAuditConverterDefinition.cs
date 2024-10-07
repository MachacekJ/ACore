namespace ACore.Server.Modules.AuditModule.Storage.ValueConverters;

public interface IAuditConverterDefinition
{
  TAudit? ToAuditValue<TAudit, TCSharp>(object? value);
  TCSharp ToCSharpValue<TCSharp, TAudit>(TAudit value);
}