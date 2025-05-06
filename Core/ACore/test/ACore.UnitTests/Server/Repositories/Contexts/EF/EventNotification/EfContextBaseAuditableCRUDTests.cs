using ACore.Server.Services;
using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.Auditable;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification;

public class EfContextBaseAuditableCRUDTests() : EfContextBaseCRUDTests(CRUDEntityTypeEnum.FakeAuditableLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateAuditableDbContextBaseAsSut(mediator);
    var en = new FakeAuditableEntity
    {
      TestProp = "test"
    };

    // Act.
    await sut.Save<FakeAuditableEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications, out var testProp);
    testProp.NewValue.Should().Be(en.TestProp);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    const string fakeDataOld = "fakeDataOld";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeAuditableEntity
    {
      TestProp = fakeDataOld
    };
    var fakeEntityUpdate = new FakeAuditableEntity();

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
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
    AssertUpdate(allNotifications, out var testProp);
    testProp.NewValue.Should().Be(fakeData);
    testProp.OldValue.Should().Be(fakeDataOld);
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
    var mediator = new Mock<IACoreServerCurrentScope>();
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
    AssertDelete(allNotifications, out var testProp);
    testProp.OldValue.Should().Be(fakeData);
  }
}