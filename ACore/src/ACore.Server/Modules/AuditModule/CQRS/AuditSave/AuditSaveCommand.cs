using ACore.Models.Result;
using ACore.Server.Storages.Models.EntityEvent;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveCommand(EntityEventItem entityEventItem) : AuditModuleRequest<Result>
{
  public EntityEventItem EntityEventItem => entityEventItem;
}