using ACore.Models.Result;
using ACore.Server.Modules.SecurityModule.CQRS.SecurityGetCurrentUser;
using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Storages.CQRS.Notifications;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF;

public class DbContextBaseTests
{
  protected readonly UserData FakeUser = new(UserTypeEnum.Test, "1", "testUser");


  protected void SetupLoggedUser(Mock<IMediator> mediator)
  {
    var result = Result.Success(FakeUser);
    mediator
      .Setup(i => i.Send(It.IsAny<SecurityGetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => result);
  }
  
  protected void SetupSaveNotification(Mock<IMediator> mediator, List<INotification>? notifications = null)
  {
    mediator
      .Setup(i => i.Publish(It.IsAny<EntityEventNotification>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification); });
  }
  
  protected static INotification AssertOneNotification(List<INotification> allNotifications)
  {
    allNotifications.Should().ContainSingle();
    var notification = allNotifications.Single();
    notification.Should().BeOfType<EntityEventNotification>();
    return notification;
  }
}