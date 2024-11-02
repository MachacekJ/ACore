using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.Auditable;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.CRUDTests;

public class DbContextBaseAuditableCRUDTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeAuditableLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator);
    var en = new FakeAuditableLongEntity();

    // Act.
    await sut.Save<FakeAuditableLongEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeAuditableLongEntity();
    var fakeEntityUpdate = new FakeAuditableLongEntity();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
      fakeEntityUpdate.Id = fakeEntityInit.Id;
    });

    fakeEntityUpdate.TestProp = fakeData;

    // Act.
    await sut.Save<FakeAuditableLongEntity, long>(fakeEntityUpdate);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertUpdate(allNotifications, fakeData);
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntity = new FakeAuditableLongEntity
    {
      Id = 1,
      TestProp = fakeData
    };

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeAuditableLongEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, fakeData);
  }
}