using MediatR;

namespace ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher.FakeClasses;

public record LongNotification(TimeSpan Duration) : INotification;