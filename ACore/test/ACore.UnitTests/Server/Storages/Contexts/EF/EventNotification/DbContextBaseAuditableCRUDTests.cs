using ACore.Server.Services.AppUser;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.Auditable;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification;

public class DbContextBaseAuditableCRUDTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeAuditableLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator);
    var en = new FakeAuditableEntity();

    // Act.
    await sut.Save<FakeAuditableEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeAuditableEntity();
    var fakeEntityUpdate = new FakeAuditableEntity();

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
      fakeEntityUpdate.Id = fakeEntityInit.Id;
    });

    fakeEntityUpdate.TestProp = fakeData;

    // Act.
    await sut.Save<FakeAuditableEntity, long>(fakeEntityUpdate);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertUpdate(allNotifications, fakeData);
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntity = new FakeAuditableEntity
    {
      Id = 1,
      TestProp = fakeData
    };

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeAuditableEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, fakeData);
  }
}