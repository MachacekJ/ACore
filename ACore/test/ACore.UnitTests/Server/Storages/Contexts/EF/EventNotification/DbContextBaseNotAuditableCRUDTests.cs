using ACore.Server.Services.AppUser;
using ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.NotAuditable;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification;

public class DbContextBaseNotAuditableCRUDTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditableLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator);
    var en = new FakeNotAuditableEntity();

    // Act.
    await sut.Save<FakeNotAuditableEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeNotAuditableEntity();
    var fakeEntityUpdate = new FakeNotAuditableEntity();

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
      fakeEntityUpdate.Id = fakeEntityInit.Id;
    });

    fakeEntityUpdate.TestProp = fakeData;

    // Act.
    await sut.Save<FakeNotAuditableEntity, long>(fakeEntityUpdate);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertUpdate(allNotifications, fakeData);
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntity = new FakeNotAuditableEntity
    {
      Id = 1,
      TestProp = fakeData
    };

    // Arrange
    var mediator = new Mock<IApp>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeNotAuditableEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, fakeData);
  }
}