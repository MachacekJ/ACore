using ACore.Server.Services;
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
    var serverOptions = new Mock<IACoreServerApp>();
    SetupSaveNotification(serverOptions, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(serverOptions);
    var en = new FakeNotAuditableEntity();

    // Act.
    await sut.Save<FakeNotAuditableEntity, long>(en);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertAdd(allNotifications, out var testProp);
    testProp.NewValue.Should().Be(en.TestProp);
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    const string fakeData = "fakeData";
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new FakeNotAuditableEntity();
    var fakeEntityUpdate = new FakeNotAuditableEntity();

    // Arrange
    var serveOptions = new Mock<IACoreServerApp>();
    SetupSaveNotification(serveOptions, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(serveOptions, impl =>
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
    AssertUpdate(allNotifications, out var testProp);
    testProp.NewValue.Should().Be(fakeData);
    testProp.OldValue.Should().BeNull();
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
    var serveOptions = new Mock<IACoreServerApp>();
    SetupSaveNotification(serveOptions, allNotifications);
    var sut = CreateNotAuditableDbContextBaseAsSut(serveOptions, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeNotAuditableEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, out var testProp);
    testProp.OldValue.Should().Be(fakeData);
  }
}