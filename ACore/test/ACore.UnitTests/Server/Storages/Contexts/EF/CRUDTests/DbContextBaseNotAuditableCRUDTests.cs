using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.NotAuditable;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.CRUDTests;

public class DbContextBaseNotAuditableCRUDTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditableLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator);
    var en = new FakeNotAuditableLongEntity();

    // Act.
    await sut.Save<FakeNotAuditableLongEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeNotAuditableLongEntity();
    var fakeEntityUpdate = new FakeNotAuditableLongEntity();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
      fakeEntityUpdate.Id = fakeEntityInit.Id;
    });

    fakeEntityUpdate.TestProp = fakeData;

    // Act.
    await sut.Save<FakeNotAuditableLongEntity, long>(fakeEntityUpdate);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertUpdate(allNotifications, fakeData);
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntity = new FakeNotAuditableLongEntity
    {
      Id = 1,
      TestProp = fakeData
    };

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeNotAuditableLongEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, fakeData);
  }
}