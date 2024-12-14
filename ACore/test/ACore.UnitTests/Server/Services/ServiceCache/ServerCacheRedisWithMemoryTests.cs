using System.Text;
using System.Text.Json;
using ACore.Models.Cache;
using ACore.Server.Repository.Configuration.RepositoryTypes;
using ACore.Server.Services.ServerCache.Configuration;
using ACore.Server.Services.ServerCache.CQRS.Notification;
using ACore.Server.Services.ServerCache.Implementations;
using ACore.Server.Services.ServerCache.Models;
using ACore.Services.Cache;
using ACore.Services.Cache.Configuration;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;

namespace ACore.UnitTests.Server.Services.ServiceCache;

public class ServerCacheRedisWithMemoryTests
{
  private readonly CacheCategory _category = new CacheCategory("category");

  private readonly ServerCacheOptions _serverCacheOptions = new ServerCacheOptions(
    new RepositoryRedisOptions("fake", "fake"));

  public ServerCacheRedisWithMemoryTests()
  {
    _serverCacheOptions.Categories.Add(_category);
  }

  [Fact]
  public async Task SetTest()
  {
    // Arrange
    var aCoreCacheFakeMocked = new Mock<IACoreCache>();
    aCoreCacheFakeMocked
      .Setup(i => i.Set(It.IsAny<CacheKey>(), It.IsAny<int>(), It.IsAny<TimeSpan?>()));
    var aDistCacheMocked = new Mock<IDistributedCache>();
    aDistCacheMocked
      .Setup(i => i.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None))
      .Returns(Task.CompletedTask);

    var notifications = new List<ServerCacheAddItemNotification>();
    var sut = ServerCacheRedisWithMemoryAsSut(notifications, aCoreCacheFakeMocked.Object, aDistCacheMocked.Object);

    // Act
    await sut.Set(CacheKey.Create(_category, "ACoreCache"), 10);

    // Assert
    notifications.Should().HaveCount(2);
    notifications.Should().Contain(a => a.CacheType == ServerCacheTypeEnum.Memory);
    notifications.Should().Contain(a => a.CacheType == ServerCacheTypeEnum.Redis);
  }

  [Theory]
  [InlineData(false, false)]
  [InlineData(false, true)]
  [InlineData(true, false)]
  [InlineData(true, true)]
  public async Task GetNoCacheTest(bool aCoreCache, bool aDistCache)
  {
    // Arrange
    var aCoreCacheFakeMocked = new Mock<IACoreCache>();
    string? aCoreCacheValue = null;
    aCoreCacheFakeMocked
      .Setup(i => i.TryGetValue(It.IsAny<CacheKey>(), out aCoreCacheValue)).Returns(aCoreCache);

    var nullString = !aDistCache ? null : Encoding.UTF8.GetBytes(JsonSerializer.Serialize("aDistCache"));
    var aDistCacheMocked = new Mock<IDistributedCache>();
    aDistCacheMocked
      .Setup(i => i.GetAsync(It.IsAny<string>(), CancellationToken.None))
      .Returns(Task.FromResult(nullString));

    var notifications = new List<ServerCacheGetItemNotification>();
    var sut = ServerCacheRedisWithMemoryAsSut(notifications, aCoreCacheFakeMocked.Object, aDistCacheMocked.Object);

    // Act
    await sut.Get<string>(CacheKey.Create(_category, "ACoreCache"));

    // Assert
    notifications.Should().HaveCount(aCoreCache ? 1 : 2);
    notifications.Single(set => set.CacheType == ServerCacheTypeEnum.Memory).Result.Should().Be(aCoreCache);
    if (!aCoreCache)
      notifications.Single(set => set.CacheType == ServerCacheTypeEnum.Redis).Result.Should().Be(aDistCache);
  }
  
  private ServerCacheRedisWithMemory ServerCacheRedisWithMemoryAsSut<T>(List<T>? notifications, IACoreCache aCoreCache, IDistributedCache distributedCache)
    where T : class, INotification
  {
    var mediatorMocked = new Mock<IMediator>();
    mediatorMocked
      .Setup(i => i.Publish(It.IsAny<T>(), It.IsAny<CancellationToken>()))
      .Callback<INotification, CancellationToken>((notification, _) => { notifications?.Add(notification as T ?? throw new NullReferenceException()); });


    var aCoreCacheOptionsMocked = new Mock<IOptions<ACoreCacheOptions>>();
    aCoreCacheOptionsMocked.Setup(o => o.Value).Returns(new ACoreCacheOptions());

    var serverCacheOptionsMocked = new Mock<IOptions<ServerCacheOptions>>();
    serverCacheOptionsMocked.Setup(o => o.Value).Returns(_serverCacheOptions);

    return new ServerCacheRedisWithMemory(aCoreCache, distributedCache, mediatorMocked.Object, aCoreCacheOptionsMocked.Object, serverCacheOptionsMocked.Object);
  }
}