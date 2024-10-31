using ACore.Base.CQRS.Results;
using ACore.Extensions;
using ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;
using ACore.Server.Modules.ICAMModule.Models;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Models.EntityEvent;
using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF;

public class DbContextBaseTests
{
  private readonly UserData _fakeUser = new(UserTypeEnum.Test, "1", "testUser");

  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator);
    var en = new FakeLongEntity();

    // Act.
    await sut.Save<FakeLongEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    allNotifications.Should().ContainSingle();

    var notification = allNotifications.Single();
    notification.Should().BeOfType<EntityEventNotification>();

    var entitySaveNotification = notification as EntityEventNotification;
    entitySaveNotification.Should().NotBeNull();
    entitySaveNotification?.EntityEvent.EntityState.Should().Be(EntityEventEnum.Added);
    entitySaveNotification?.EntityEvent.IsAuditable.Should().BeFalse();
    entitySaveNotification?.EntityEvent.Version.Should().Be(0);
    entitySaveNotification?.EntityEvent.TableName.Should().Be(nameof(FakeLongEntity));
    entitySaveNotification?.EntityEvent.SchemaName.Should().BeNull();
    entitySaveNotification?.EntityEvent.PkValue.Should().Be(1);
    entitySaveNotification?.EntityEvent.PkValueString.Should().BeNull();
    entitySaveNotification?.EntityEvent.UserId.Should().Be(_fakeUser.ToString());

    entitySaveNotification?.EntityEvent.ChangedColumns.Should().HaveCount(2);

    var idProp = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeLongEntity.Id));
    idProp.Should().NotBeNull();
    idProp?.ColumnName.Should().Be(nameof(FakeLongEntity.Id));
    idProp?.IsChanged.Should().BeTrue();
    idProp?.IsAuditable.Should().BeFalse();
    idProp?.DataType.Should().Be(typeof(long).ACoreTypeName());
    idProp?.NewValue.Should().Be(1);
    idProp?.OldValue.Should().BeNull();

    var prop1Prop = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeLongEntity.TestProp));
    prop1Prop.Should().NotBeNull();
    prop1Prop?.ColumnName.Should().Be(nameof(FakeLongEntity.TestProp));
    prop1Prop?.IsChanged.Should().BeTrue();
    prop1Prop?.IsAuditable.Should().BeFalse();
    prop1Prop?.DataType.Should().Be(typeof(string).ACoreTypeName());
    prop1Prop?.NewValue.Should().BeNull();
    prop1Prop?.OldValue.Should().BeNull();
  }

  private FakeDbContextBaseImpl CreateDbContextBaseAsSut(Mock<IMediator> mediator)
  {
    SetupLoggedUser(mediator);
    var dbContextOptions = new DbContextOptions<FakeDbContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeDbContextBaseImpl>().LoggerMocked;
    var res = new FakeDbContextBaseImpl(dbContextOptions, mediator.Object, loggerMocked);
    return res;
  }

  private void SetupLoggedUser(Mock<IMediator> mediator)
  {
    var result = Result.Success(_fakeUser);
    mediator
      .Setup(i => i.Send(It.IsAny<ICAMGetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => result);
  }

  private void SetupSaveNotification(Mock<IMediator> mediator, List<INotification>? notifications = null)
  {
    mediator
      .Setup(i => i.Publish(It.IsAny<EntityEventNotification>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification); });
  }
}