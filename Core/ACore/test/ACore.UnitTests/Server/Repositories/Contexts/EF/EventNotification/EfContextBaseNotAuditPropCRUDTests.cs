using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Services;
using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.NotAuditProp;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification;


/// <summary>
/// Test for entity that prop contains <see cref="NotAuditableAttribute"/>. 
/// </summary>
public class EfContextBaseNotAuditPropCRUDTests() : EfContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditPropLongEntity)
{
  [Fact]
  public async Task AddNewItemTest()
  {
    var allNotifications = new List<INotification>();

    // Arrange
    var serveOptions = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(serveOptions, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(serveOptions);
    var en = new FakeNotAuditPropEntity();

    // Act.
    await sut.Save<FakeNotAuditPropEntity, long>(en);

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
    var fakeEntityInit = new FakeNotAuditPropEntity();
    var fakeEntityUpdate = new FakeNotAuditPropEntity();

    // Arrange
    var serveOptions = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(serveOptions, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(serveOptions, impl =>
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
    AssertUpdate(allNotifications, out var testProp);
    testProp.NewValue.Should().Be(fakeData);
    testProp.OldValue.Should().BeNull();
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
    var serveOptions = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(serveOptions, allNotifications);
    var sut = CreateNotAuditPropDbContextBaseAsSut(serveOptions, impl =>
    {
      impl.Fakes.Add(fakeEntity);
      impl.SaveChanges();
    });

    fakeEntity.TestProp = fakeData;

    // Act.
    await sut.Delete<FakeNotAuditPropEntity, long>(fakeEntity.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertDelete(allNotifications, out var testProp);
    testProp.OldValue.Should().Be(fakeData);
  }
}