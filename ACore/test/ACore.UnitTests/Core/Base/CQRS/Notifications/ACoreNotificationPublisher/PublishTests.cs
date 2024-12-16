using System.Diagnostics;
using ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher;

public class PublishTests
{
  /// <summary>
  /// When some task throws an exception, this exception must be thrown up and must not be hidden.
  /// </summary>
  [Fact]
  public async Task ThrowExceptionTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<ThrowNotificationHandler>();
    var throwNotification = new ThrowNotification();
    var allHandlers = AllHandlers(loggerHelper.LoggerMocked, true);
    var aCoreNotificationPublisherSut = CreateNotificationPublisherAsSut();

    // Act
    Func<Task> ac = async () => await aCoreNotificationPublisherSut.Publish(allHandlers, throwNotification, CancellationToken.None);

    // Assert
    await ac.Should().ThrowAsync<NotImplementedException>();
  }

  [Fact]
  public async Task NotThrowExceptionTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<ThrowNotificationHandler>();
    var throwNotification = new ThrowNotification();
    var allHandlers = AllHandlers(loggerHelper.LoggerMocked, false);
    var sut = CreateNotificationPublisherAsSut();

    // Act
    await sut.Publish(allHandlers, throwNotification, CancellationToken.None);

    // Assert
    loggerHelper.LogLevels.Should().HaveCountGreaterThan(0);
    loggerHelper.LogMessages.Should().HaveCountGreaterThan(0);
    loggerHelper.LogExceptions.Where(e => e.Message == new NotImplementedException().Message).Should().HaveCount(2);
  }

  [Fact]
  public async Task NotInBackgroundLongTest()
  {
    var duration = TimeSpan.FromMilliseconds(100);
    
    // Arrange
    var loggerHelper = new MoqLogger<LongNotificationHandler>();
    var longNotification = new LongNotification(duration);

    var allHandlers = AllLongHandlers(loggerHelper.LoggerMocked, false);
    var sut = CreateNotificationPublisherAsSut();
    var sw = new Stopwatch();
    
    // Act
    sw.Start();
    await sut.Publish(allHandlers, longNotification, CancellationToken.None);
    sw.Stop();
    
    // Assert
    sw.ElapsedTicks.Should().BeGreaterOrEqualTo(duration.Ticks);
  }
  
  [Fact]
  public async Task InBackgroundToLongTest()
  {
    var duration = TimeSpan.FromMilliseconds(100);
    
    // Arrange
    var loggerHelper = new MoqLogger<LongNotificationHandler>();
    var longNotification = new LongNotification(duration);

    var allHandlers = AllLongHandlers(loggerHelper.LoggerMocked, true);
    var sut = CreateNotificationPublisherAsSut();
    var sw = new Stopwatch();
    
    // Act
    sw.Start();
    await sut.Publish(allHandlers, longNotification, CancellationToken.None);
    sw.Stop();
    
    // Assert
    sw.ElapsedTicks.Should().BeLessThan(duration.Ticks);
  }

  private static ACore.CQRS.Notifications.ACoreNotificationPublisher CreateNotificationPublisherAsSut()
    => new();

  private static List<NotificationHandlerExecutor> AllHandlers(ILogger<ThrowNotificationHandler> loggerHelper, bool throwExceptions)
  {
    var throwNotificationHandler = new ThrowNotificationHandler(loggerHelper, throwExceptions);
    var notificationHandlerExecutors = new List<NotificationHandlerExecutor>
    {
      new(throwNotificationHandler, (notification, cancellationToken) => throwNotificationHandler.Handle(notification as ThrowNotification ?? throw new InvalidOperationException(), cancellationToken)),
      new(throwNotificationHandler, (notification, cancellationToken) => throwNotificationHandler.Handle(notification as ThrowNotification ?? throw new InvalidOperationException(), cancellationToken))
    };
    return notificationHandlerExecutors;
  }

  private static List<NotificationHandlerExecutor> AllLongHandlers(ILogger<LongNotificationHandler> loggerHelper, bool inBackground)
  {
    var longNotificationHandler = new LongNotificationHandler(loggerHelper, inBackground);
    var notificationHandlerExecutors = new List<NotificationHandlerExecutor>
    {
      new(longNotificationHandler, (notification, cancellationToken) => longNotificationHandler.Handle(notification as LongNotification ?? throw new InvalidOperationException(), cancellationToken)),
    };
    return notificationHandlerExecutors;
  }
}