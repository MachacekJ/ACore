using ACore.Server.Services;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.NotAuditProp;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification;

public class DbContextBaseNotAuditPropCRUDTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditPropLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IACoreServerApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(mediator);
    var en = new FakeNotAuditPropEntity();

    // Act.
    await sut.Save<FakeNotAuditPropEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeNotAuditPropEntity();
    var fakeEntityUpdate = new FakeNotAuditPropEntity();

    // Arrange
    var mediator = new Mock<IACoreServerApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
      fakeEntityUpdate.Id = fakeEntityInit.Id;
    });

    fakeEntityUpdate.TestProp = fakeData;

    // Act.
    await sut.Save<FakeNotAuditPropEntity, long>(fakeEntityUpdate);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertUpdate(allNotifications, fakeData);
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntity = new FakeNotAuditPropEntity
    {
      Id = 1,
      TestProp = fakeData,
    };

    // Arrange
    var mediator = new Mock<IACoreServerApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeNotAuditPropEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, fakeData);
  }
}