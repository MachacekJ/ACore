using ACore.Results;
using ACore.Server.Repository.Models.EntityEvent;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveCommand(EntityEventItem entityEventItem) : AuditModuleRequest<Result>
{
  public EntityEventItem EntityEventItem => entityEventItem;
}