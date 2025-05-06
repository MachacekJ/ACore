using ACore.CQRS.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.CQRS.Extensions;

public static class MediatrServiceConfigurationExtensions
{
  public static void ParallelNotification(this MediatRServiceConfiguration config)
  {
    // Setting the publisher directly will make the instance a Singleton.
    config.NotificationPublisher = new ACoreNotificationPublisher();
    config.NotificationPublisherType = typeof(ACoreNotificationPublisher);
  }
}