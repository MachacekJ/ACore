using ACore.CQRS.Notifications;
using ACore.Server.Modules.AuditModule.CQRS.AuditSave;
using ACore.Server.Repository.CQRS.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.CQRS.Notifications;

public class RepositorySaveNotificationHandler(ILogger<RepositorySaveNotificationHandler> logger, IMediator mediator) : ACoreNotificationHandler<RepositorySaveEventNotification>(logger)
{
  public override bool ThrowException => true;
  public override bool InBackground => false;

  protected override async Task HandleMethod(RepositorySaveEventNotification notification, CancellationToken cancellationToken)
  {
    if (notification.EntityEvent.IsAuditable)
      await mediator.Send(new AuditSaveCommand(notification.EntityEvent), cancellationToken);
  }
}