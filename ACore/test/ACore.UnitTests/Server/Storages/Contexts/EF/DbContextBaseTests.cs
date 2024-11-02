using ACore.Base.CQRS.Results;
using ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;
using ACore.Server.Modules.ICAMModule.Models;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.Auditable;
using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.NotAuditable;
using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.NotAuditProp;
using ACore.UnitTests.TestImplementations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF;

public class DbContextBaseTests
{
  protected readonly UserData FakeUser = new(UserTypeEnum.Test, "1", "testUser");

  protected FakeNotAuditPropDbContextBaseImpl CreateNotAuditPropDbContextBaseAsSut(Mock<IMediator> mediator, Action<FakeNotAuditPropDbContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(mediator);
    var dbContextOptions = new DbContextOptions<FakeNotAuditPropDbContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeNotAuditPropDbContextBaseImpl>().LoggerMocked;
    var res = new FakeNotAuditPropDbContextBaseImpl(dbContextOptions, mediator.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  protected FakeAuditableDbContextBaseImpl CreateAuditableDbContextBaseAsSut(Mock<IMediator> mediator, Action<FakeAuditableDbContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(mediator);
    var dbContextOptions = new DbContextOptions<FakeAuditableDbContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeAuditableDbContextBaseImpl>().LoggerMocked;
    var res = new FakeAuditableDbContextBaseImpl(dbContextOptions, mediator.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  protected FakeNotAuditableDbContextBaseImpl CreateNotAuditableDbContextBaseAsSut(Mock<IMediator> mediator, Action<FakeNotAuditableDbContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(mediator);
    var dbContextOptions = new DbContextOptions<FakeNotAuditableDbContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeNotAuditableDbContextBaseImpl>().LoggerMocked;
    var res = new FakeNotAuditableDbContextBaseImpl(dbContextOptions, mediator.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }
  
  private void SetupLoggedUser(Mock<IMediator> mediator)
  {
    var result = Result.Success(FakeUser);
    mediator
      .Setup(i => i.Send(It.IsAny<ICAMGetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => result);
  }

  protected void SetupSaveNotification(Mock<IMediator> mediator, List<INotification>? notifications = null)
  {
    mediator
      .Setup(i => i.Publish(It.IsAny<EntityEventNotification>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification); });
  }
}