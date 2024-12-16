using ACore.Models.Cache;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Services.ServerCache.CQRS.Notification;
using ACore.Server.Services.ServerCache.Implementations;
using ACore.Server.Services.ServerCache.Models;
using ACore.Server.Storages.Configuration;
using ACore.Services.ACoreCache;
using ACore.Services.ACoreCache.Configuration;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;

namespace ACore.UnitTests.Server.Services.ServiceCache;

public class ServerCacheRedisWithMemoryTests
{
  [Fact]
  public async Task SetTest()
  {
    var category = new CacheCategory("category");
    var ser = new ServerCacheOptions(
      new StorageRedisOptions("fake", "fake"));
    ser.Categories.Add(category);

    // Arrange
    var notifications = new List<ServerCacheAddItemNotification>();
    var sut = ServerCacheRedisWithMemoryAsSut(notifications, new ACoreCacheOptions(), ser);

    // Act
    await sut.Set(CacheKey.Create(category, "ACoreCache"), 10);

    // Assert
    notifications.Should().HaveCount(2);
    notifications.Should().Contain(a => a.CacheType == ServerCacheTypeEnum.Memory);
    notifications.Should().Contain(a => a.CacheType == ServerCacheTypeEnum.Redis);
  }

  private ServerCacheRedisWithMemory ServerCacheRedisWithMemoryAsSut(List<ServerCacheAddItemNotification>? notifications, ACoreCacheOptions acoreCacheOptions, ServerCacheOptions serverCacheOptions)
  {
    var fakeMediator = new Mock<IMediator>();
    fakeMediator
      .Setup(i => i.Publish(It.IsAny<ServerCacheAddItemNotification>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification as ServerCacheAddItemNotification ?? throw new NullReferenceException()); });

    var aCoreCacheFake = new Mock<IACoreCache>();
    aCoreCacheFake.Setup(i => i.Set(It.IsAny<CacheKey>(), It.IsAny<int>(), It.IsAny<TimeSpan?>()));

    var aDistCacheFake = new Mock<IDistributedCache>();
    aDistCacheFake
      .Setup(i => i.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None))
      .Returns(Task.CompletedTask);

    var op = new Mock<IOptions<ACoreCacheOptions>>();
    op.Setup(o => o.Value).Returns(acoreCacheOptions);

    var op2 = new Mock<IOptions<ServerCacheOptions>>();
    op2.Setup(o => o.Value).Returns(serverCacheOptions);

    return new ServerCacheRedisWithMemory(aCoreCacheFake.Object, aDistCacheFake.Object, fakeMediator.Object, op.Object, op2.Object);
  }
}