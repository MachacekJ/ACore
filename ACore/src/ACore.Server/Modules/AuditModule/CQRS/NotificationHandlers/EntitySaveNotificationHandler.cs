using ACore.CQRS.Notifications;
using ACore.Server.Modules.AuditModule.CQRS.AuditSave;
using ACore.Server.Storages.CQRS.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.CQRS.NotificationHandlers;

public class EntitySaveNotificationHandler(ILogger<EntitySaveNotificationHandler> logger, IMediator mediator) : ACoreNotificationHandler<EntityEventNotification>(logger)
{
  public override bool ThrowException => true;

  protected override async Task HandleMethod(EntityEventNotification notification, CancellationToken cancellationToken)
  {
    if (notification.EntityEvent.IsAuditable)
      await mediator.Send(new AuditSaveCommand(notification.EntityEvent), cancellationToken);
  }
}