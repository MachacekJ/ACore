using ACore.CQRS.Notifications;
using ACore.Server.Modules.AuditModule.CQRS.AuditSave;
using ACore.Server.Storages.CQRS.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.CQRS.Notifications;

public class EntitySaveNotificationHandler(ILogger<EntitySaveNotificationHandler> logger, IMediator mediator) : ACoreNotificationHandler<EntityEventNotification>(logger)
{
  public override bool ThrowException => true;
  public override bool InBackground => false;

  protected override async Task HandleMethod(EntityEventNotification notification, CancellationToken cancellationToken)
  {
    if (notification.EntityEvent.IsAuditable)
      await mediator.Send(new AuditSaveCommand(notification.EntityEvent), cancellationToken);
  }
}