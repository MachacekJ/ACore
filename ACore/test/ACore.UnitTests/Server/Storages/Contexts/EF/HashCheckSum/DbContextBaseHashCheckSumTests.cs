using ACore.Extensions;
using ACore.Models.Result;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.UnitTests.Server.Storages.Contexts.EF.HashCheckSum.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.HashCheckSum;

public class DbContextBaseHashCheckSumTests : DbContextBaseTests
{
  private const string HashFake = "hashFake";

  [Fact]
  // Item has not been saved because is the same. 
  public async Task CheckSumHashAddItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity
    {
      TestProp = "test",
      ExcludeFromHash = "exclude",
    };

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator);

    // Act.
    var res = await sut.Save<WorkWithHashEntity, int>(fakeEntityInit);
    fakeEntityInit.ExcludeFromHash = "";

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertOneNotification(allNotifications);

    res.DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.Added);
    res.SumHash.Should().Be(fakeEntityInit.GetSumHash(HashFake));
  }

  [Fact]
  public async Task CheckSumHashUpdateItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      // must be copied, entity framework work with tracking and fakeEntityInit is changed by fakeEntityUpdate, because EF uses it
      var nee = new WorkWithHashEntity();
      fakeEntityInit.Adapt(nee);
      impl.Fakes.Add(nee);
      impl.SaveChanges();
      fakeEntityInit.Id = nee.Id;
    });

    //  fakeEntityInit.Adapt(fakeEntityUpdate);
    fakeEntityUpdate.Id = fakeEntityInit.Id;
    fakeEntityUpdate.TestProp = "update";
    // Act.
    var hash = fakeEntityInit.GetSumHash(HashFake);
    var res = await sut.Save<WorkWithHashEntity, int>(fakeEntityUpdate, hash);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertOneNotification(allNotifications);
    res.IsSuccess.Should().BeTrue();
    res.SumHash.Should().NotBe(fakeEntityInit.GetSumHash(HashFake));
    res.DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.Modified);
  }

  [Fact]
  public async Task CheckSumHashDeleteItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
    });

    // Act.
    var res = await sut.Delete<WorkWithHashEntity, int>(fakeEntityInit.Id);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertOneNotification(allNotifications);
    res.IsSuccess.Should().BeTrue();
    res.DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.Deleted);
    res.SumHash.Should().BeNull();
  }

  [Fact]
  // Item has not been saved because is the same. 
  public async Task NotSaveSumHashIsTheSameAsSavedValueUpdateItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
    });

    // Init and update item are identical.
    fakeEntityInit.Adapt(fakeEntityUpdate);

    // Act.
    var hash = fakeEntityInit.GetSumHash(HashFake);
    var res = await sut.Save<WorkWithHashEntity, int>(fakeEntityUpdate, hash);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    allNotifications.Should().BeEmpty();
    res.IsSuccess.Should().BeTrue();
    res.SumHash.Should().Be(fakeEntityInit.GetSumHash(HashFake));
    res.DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.UnModified);
  }

  [Fact]
  // Item has not been saved because is the same. 
  public async Task InvalidSumHashIsTheSameAsSavedValueUpdateItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();
    var hash = "invalidHash";

    // Arrange
    var mediator = new Mock<IMediator>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
    });

    // Init and update item are identical.
    fakeEntityInit.Adapt(fakeEntityUpdate);

    // Act.
    var res = await sut.Save<WorkWithHashEntity, int>(fakeEntityUpdate, hash);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    allNotifications.Should().BeEmpty();
    res.IsSuccess.Should().BeFalse();
    res.SumHash.Should().BeNull();
    res.DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.Unknown);
    res.ResultErrorItem.Code.Should().Be("concurrency");
  }

  private WorkWithHashDbContextBaseImpl CreateDbContextBaseAsSut(Mock<IMediator> mediator, Action<WorkWithHashDbContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(mediator);
    SetupAppOptionQuery(mediator);
    var dbContextOptions = new DbContextOptions<WorkWithHashDbContextBaseImpl>();
    var loggerMocked = new MoqLogger<WorkWithHashDbContextBaseImpl>().LoggerMocked;
    var res = new WorkWithHashDbContextBaseImpl(dbContextOptions, mediator.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  private void SetupAppOptionQuery(Mock<IMediator> mediator)
  {
    var result = Result.Success(HashFake);
    mediator
      .Setup(i => i.Send(It.IsAny<AppOptionQuery<string>>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => result);
  }
}