using ACore.CQRS.Notifications;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher.FakeClasses;

public class LongNotificationHandler(ILogger<LongNotificationHandler> logger, bool inBackground) : ACoreNotificationHandler<LongNotification>(logger)
{
  public override bool ThrowException => true;
  public override bool InBackground => inBackground;

  protected override async Task HandleMethod(LongNotification notification, CancellationToken cancellationToken)
  {
    await Task.Delay(notification.Duration, cancellationToken);
  }
}