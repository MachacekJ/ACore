using ACore.Extensions;
using ACore.Server.Configuration;
using ACore.Server.Repository.Results;
using ACore.Server.Repository.Results.Models;
using ACore.Server.Services;
using ACore.UnitTests.Server.Repositories.Contexts.EF.HashCheckSum.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.HashCheckSum;

public class EfContextBaseHashCheckSumTests : EfContextBaseTests
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
    var mediator = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator);

    // Act.
    var res = await sut.Save<WorkWithHashEntity, int>(fakeEntityInit);
    fakeEntityInit.ExcludeFromHash = "";

    // Assert
    sut.Fakes.Count().Should().Be(1);
    AssertOneNotification(allNotifications);

    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.Added);
    res.SumHash.Should().Be(fakeEntityInit.GetSumHash(HashFake));
  }

  [Fact]
  public async Task CheckSumHashUpdateItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
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
    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.Modified);
  }

  [Fact]
  public async Task CheckSumHashDeleteItemTest()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
    });

    // Act.
    var hash = fakeEntityInit.GetSumHash(HashFake);
    var res = await sut.Delete<WorkWithHashEntity, int>(fakeEntityInit.Id, hash);

    // Assert
    sut.Fakes.Count().Should().Be(0);
    AssertOneNotification(allNotifications);
    res.IsSuccess.Should().BeTrue();
    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.Deleted);
    res.SumHash.Should().BeNull();
  }

  [Fact]
  // Item has not been saved because is the same. 
  public async Task CheckSumHashUpdateItemTest_TheSameHashCode()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
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
    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.UnModified);
  }

  [Fact]
  // Item has not been saved because is the same. 
  public async Task CheckSumHashUpdateItemTest_InvalidHashCode()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };
    var fakeEntityUpdate = new WorkWithHashEntity();
    var hash = "invalidHash";

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
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
    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.Failed);
    res.ResultErrorItem.Code.Should().Be(RepositoryOperationResult.ErrorCodeConcurrency);
  }
  
  [Fact]
  public async Task CheckSumHashDeleteItemTest_InvalidHashCode()
  {
    var allNotifications = new List<INotification>();
    var fakeEntityInit = new WorkWithHashEntity { TestProp = "test" };

    // Arrange
    var mediator = new Mock<IACoreServerCurrentScope>();
    SetupSaveNotification(mediator, allNotifications);
    var sut = CreateDbContextBaseAsSut(mediator, impl =>
    {
      impl.Fakes.Add(fakeEntityInit);
      impl.SaveChanges();
    });

    // Act.
    var hash = "anotherHash";
    var res = await sut.Delete<WorkWithHashEntity, int>(fakeEntityInit.Id, hash);

    // Assert
    sut.Fakes.Count().Should().Be(1);
    allNotifications.Should().BeEmpty();
    res.IsSuccess.Should().BeFalse();
    res.IsFailure.Should().BeTrue();
    res.SumHash.Should().BeNull();
    res.RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.Failed);
    res.ResultErrorItem.Code.Should().Be(RepositoryOperationResult.ErrorCodeConcurrency);
  }

  private WorkWithHashEfContextBaseImpl CreateDbContextBaseAsSut(Mock<IACoreServerCurrentScope> serverCurrentScope, Action<WorkWithHashEfContextBaseImpl>? seed = null)
  {
    SetupLoggedUser(serverCurrentScope);
    SetupAppOptionQuery(serverCurrentScope);
    var dbContextOptions = new DbContextOptions<WorkWithHashEfContextBaseImpl>();
    var loggerMocked = new MoqLogger<WorkWithHashEfContextBaseImpl>().LoggerMocked;
    var res = new WorkWithHashEfContextBaseImpl(dbContextOptions, serverCurrentScope.Object, loggerMocked);
    seed?.Invoke(res);
    return res;
  }

  private void SetupAppOptionQuery(Mock<IACoreServerCurrentScope> serverCurrentScope)
  {
    //var result = Result.Success(HashFake);
    serverCurrentScope
      .Setup(i => i.Options)
      .Returns(() => new ACoreServerOptions
      {
        SaltForHash = HashFake
      });
  }
}