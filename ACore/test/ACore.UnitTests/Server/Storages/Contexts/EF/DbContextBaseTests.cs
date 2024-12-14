using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Services.AppUser;
using ACore.Server.Storages.CQRS.Notifications;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF;

public class DbContextBaseTests
{
  protected readonly UserData FakeUser = new(UserTypeEnum.Test, "1", "testUser");


  protected void SetupLoggedUser(Mock<IApp> app)
  {
    //var result = Result.Success(FakeUser);
    app
      .Setup(i => i.CurrentUser)
      .Returns(() => FakeUser);
  }
  
  protected void SetupSaveNotification(Mock<IApp> app, List<INotification>? notifications = null)
  {
    var mediator = new Mock<IMediator>();
    mediator
      .Setup(i => i.Publish(It.IsAny<EntityEventNotification>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification); });
    
    app.Setup(i=>i.Mediator)
      .Returns(mediator.Object);
  }
  
  protected static INotification AssertOneNotification(List<INotification> allNotifications)
  {
    allNotifications.Should().ContainSingle();
    var notification = allNotifications.Single();
    notification.Should().BeOfType<EntityEventNotification>();
    return notification;
  }
}