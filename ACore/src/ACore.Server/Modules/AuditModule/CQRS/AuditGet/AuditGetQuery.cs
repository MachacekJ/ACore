using ACore.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetQuery<TPK>(string tableName, TPK pkValue, bool restoreEntity = false , string? schemaName = null) : AuditModuleRequest<Result<AuditGetDataOut<TPK>[]>>
{
  public string TableName => tableName;
  public string? SchemaName => schemaName;
  public TPK PKValue => pkValue;
}