using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Models.EntityEvent;

namespace ACore.Server.Modules.AuditModule.Repositories;

public interface IAuditRepository : IRepository
{
  Task<RepositoryOperationResult> SaveAuditAsync(EntityEventItem entityEventItem);

  Task<AuditInfoItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null);
}