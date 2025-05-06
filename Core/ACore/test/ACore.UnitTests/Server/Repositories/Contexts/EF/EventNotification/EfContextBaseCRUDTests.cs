using ACore.Extensions;
using ACore.Server.Repository.CQRS.Notifications;
using ACore.Server.Repository.Models.EntityEvent;
using ACore.Server.Services;
using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.Auditable;
using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.NotAuditable;
using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.NotAuditProp;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification;

/// <summary>
/// Must be the match with audit attribute in specific entity.
/// </summary>
public enum CRUDEntityTypeEnum
{
  FakeNotAuditableLongEntity = 0,
  FakeAuditableLongEntity = 1,
  FakeNotAuditPropLongEntity = 2
}

public abstract class EfContextBaseCRUDTests(CRUDEntityTypeEnum entityType) : EfContextBaseTests
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
  protected FakeNotAuditPropEfContextBaseImpl CreateNotAuditPropDbContextBaseAsSut(Mock<IACoreServerCurrentScope> serverCurrentScope, Action<FakeNotAuditPropEfContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(serverCurrentScope);
    var dbContextOptions = new DbContextOptions<FakeNotAuditPropEfContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeNotAuditPropEfContextBaseImpl>().LoggerMocked;
    var res = new FakeNotAuditPropEfContextBaseImpl(dbContextOptions, serverCurrentScope.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  protected FakeAuditableEfContextBaseImpl CreateAuditableDbContextBaseAsSut(Mock<IACoreServerCurrentScope> serverCurrentScope, Action<FakeAuditableEfContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(serverCurrentScope);
    var dbContextOptions = new DbContextOptions<FakeAuditableEfContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeAuditableEfContextBaseImpl>().LoggerMocked;
    var res = new FakeAuditableEfContextBaseImpl(dbContextOptions, serverCurrentScope.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  protected FakeNotAuditableEfContextBaseImpl CreateNotAuditableDbContextBaseAsSut(Mock<IACoreServerCurrentScope> serverCurrentScope, Action<FakeNotAuditableEfContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(serverCurrentScope);
    var dbContextOptions = new DbContextOptions<FakeNotAuditableEfContextBaseImpl>();
    var loggerMocked = new MoqLogger<FakeNotAuditableEfContextBaseImpl>().LoggerMocked;
    var res = new FakeNotAuditableEfContextBaseImpl(dbContextOptions, serverCurrentScope.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }
  #endregion

  #region Assert
  /// <summary>
  /// Check if add event notification has been fired.
  /// </summary>
  protected void AssertAdd(List<INotification> allNotifications, out EntityEventColumnItem testProp)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Added, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeTrue();
    idProp.NewValue.Should().Be(1);
    idProp.OldValue.Should().BeNull();

    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    prop1Prop.OldValue.Should().BeNull();
    testProp = prop1Prop;
  }
  /// <summary>
  /// Check if update event notification has been fired.
  /// </summary>
  protected void AssertUpdate(List<INotification> allNotifications, out EntityEventColumnItem testProp)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Modified, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeFalse();
    idProp.NewValue.Should().Be(1);
    idProp.OldValue.Should().Be(1);

    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    testProp = prop1Prop;
  }

  /// <summary>
  /// Check if delete event notification has been fired.
  /// </summary>
  protected void AssertDelete(List<INotification> allNotifications, out EntityEventColumnItem testProp)
  {
    var notification = AssertOneNotification(allNotifications);
    var entitySaveNotification = AssertBaseEventNotification(notification, EntityEventEnum.Deleted, _auditEntityVersion);

    var idProp = AssertEventNotificationId(entitySaveNotification);
    idProp.IsChanged.Should().BeTrue();
    idProp.NewValue.Should().BeNull();
    idProp.OldValue.Should().Be(1);
    
    var prop1Prop = AssertEventNotificationTestProp(entitySaveNotification);
    prop1Prop.NewValue.Should().BeNull();
    testProp = prop1Prop;
  }

  
  private RepositorySaveEventNotification? AssertBaseEventNotification(INotification notification, EntityEventEnum operation, int auditEntityVersion)
  {
    var entitySaveNotification = notification as RepositorySaveEventNotification;
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

  private EntityEventColumnItem AssertEventNotificationId(RepositorySaveEventNotification? entitySaveNotification)
  {
    var idProp = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeNotAuditableEntity.Id));
    idProp.Should().NotBeNull();
    idProp?.ColumnName.Should().Be(nameof(FakeNotAuditableEntity.Id));
    idProp?.IsAuditable.Should().Be(_auditable);
    idProp?.DataType.Should().Be(typeof(long).ACoreTypeName());
    return idProp ?? throw new Exception();
  }

  private EntityEventColumnItem AssertEventNotificationTestProp(RepositorySaveEventNotification? entitySaveNotification)
  {
    var prop1Prop = entitySaveNotification?.EntityEvent.ChangedColumns.FirstOrDefault(e => e.PropName == nameof(FakeNotAuditableEntity.TestProp));
    prop1Prop.Should().NotBeNull();
    prop1Prop?.ColumnName.Should().Be(nameof(FakeAuditableEntity.TestProp));
    prop1Prop?.IsChanged.Should().BeTrue();
    prop1Prop?.IsAuditable.Should().Be(
      entityType != CRUDEntityTypeEnum.FakeNotAuditPropLongEntity && _auditable);
    prop1Prop?.DataType.Should().Be(typeof(string).ACoreTypeName());
    return prop1Prop ?? throw new Exception();
  }
  #endregion
}