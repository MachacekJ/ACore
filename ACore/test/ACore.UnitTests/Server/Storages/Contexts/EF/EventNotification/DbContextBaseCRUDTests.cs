using ACore.Extensions;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Models.EntityEvent;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.Auditable;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.NotAuditable;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.NotAuditProp;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification;

/// <summary>
/// Must be the match with audit attribute in specific entity.
/// </summary>
public enum CRUDEntityTypeEnum
{
  FakeNotAuditableLongEntity = 0,
  FakeAuditableLongEntity = 1,
  FakeNotAuditPropLongEntity = 2
}

public abstract class DbContextBaseCRUDTests(CRUDEntityTypeEnum entityType) : DbContextBaseTests
{
  private readonly bool _auditable = entityType != CRUDEntityTypeEnum.FakeNotAuditableLongEntity;
  private readonly int _auditEntityVersion = Convert.ToInt32(entityType);
  private readonly string _entityName = entityType switch
  {
    CRUDEntityTypeEnum.FakeAuditableLongEntity => nameof(FakeAuditableEntity),
    CRUDEntityTypeEnum.FakeNotAuditableLongEntity => nameof(FakeNotAuditableEntity),
    CRUDEntityTypeEnum.FakeNotAuditPropLongEntity => nameof(FakeNotAuditPropEntity),
    _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
  };

  #region Arrange
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
  #endregion

  #region Assert
  protected void AssertAdd(List<INotification> allNotifications)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Added, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeTrue();
    idProp.NewValue.Should().Be(1);
    idProp.OldValue.Should().BeNull();

    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    prop1Prop.NewValue.Should().BeNull();
    prop1Prop.OldValue.Should().BeNull();
  }

  protected void AssertUpdate(List<INotification> allNotifications, string fakeData)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Modified, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeFalse();
    idProp.NewValue.Should().Be(1);
    idProp.OldValue.Should().Be(1);

    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    prop1Prop.OldValue.Should().BeNull();
    prop1Prop.NewValue.Should().Be(fakeData);
  }

  protected void AssertDelete(List<INotification> allNotifications, string fakeData)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Deleted, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeTrue();
    idProp.NewValue.Should().BeNull();
    idProp.OldValue.Should().Be(1);

    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    prop1Prop.OldValue.Should().Be(fakeData);
    prop1Prop.NewValue.Should().BeNull();
  }

  
  private EntityEventNotification? AssertBaseEventNotification(INotification notification, EntityEventEnum operation, int auditEntityVersion)
  {
    var entitySaveNotification = notification as EntityEventNotification;
    entitySaveNotification.Should().NotBeNull();
    entitySaveNotification?.EntityEvent.EntityState.Should().Be(operation);
    entitySaveNotification?.EntityEvent.IsAuditable.Should().Be(_auditable);
    entitySaveNotification?.EntityEvent.Version.Should().Be(auditEntityVersion);
    entitySaveNotification?.EntityEvent.TableName.Should().Be(_entityName);
    entitySaveNotification?.EntityEvent.SchemaName.Should().BeNull();
    entitySaveNotification?.EntityEvent.PkValue.Should().Be(1);
    entitySaveNotification?.EntityEvent.PkValueString.Should().BeNull();
    entitySaveNotification?.EntityEvent.UserId.Should().Be(FakeUser.ToString());
    entitySaveNotification?.EntityEvent.ChangedColumns.Should().HaveCount(2);
    return entitySaveNotification;
  }

  private EntityEventColumnItem AssertEventNotificationId(EntityEventNotification? entitySaveNotification)
  {
    var idProp = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeNotAuditableEntity.Id));
    idProp.Should().NotBeNull();
    idProp?.ColumnName.Should().Be(nameof(FakeNotAuditableEntity.Id));
    idProp?.IsAuditable.Should().Be(_auditable);
    idProp?.DataType.Should().Be(typeof(long).ACoreTypeName());
    return idProp ?? throw new Exception();
  }

  private EntityEventColumnItem AssertEventNotificationTestProp(EntityEventNotification? entitySaveNotification)
  {
    var prop1Prop = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeNotAuditableEntity.TestProp));
    prop1Prop.Should().NotBeNull();
    prop1Prop?.ColumnName.Should().Be(nameof(FakeNotAuditableEntity.TestProp));
    prop1Prop?.IsChanged.Should().BeTrue();
    prop1Prop?.IsAuditable.Should().Be(
      entityType != CRUDEntityTypeEnum.FakeNotAuditPropLongEntity && _auditable);
    prop1Prop?.DataType.Should().Be(typeof(string).ACoreTypeName());
    return prop1Prop ?? throw new Exception();
  }
  #endregion
}