using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Repository;
using ACore.Server.Repository.Models.EntityEvent;
using ACore.Server.Repository.Results;

namespace ACore.Server.Modules.AuditModule.Repositories;

public interface IAuditRepository : IDbRepository
{
  Task<RepositoryOperationResult> SaveAuditAsync(EntityEventItem entityEventItem);

  Task<AuditInfoItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null);
}