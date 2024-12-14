using ACore.Models.Cache;
using ACore.UnitTests.Core.Services.Cache.FakeClasses;
using FluentAssertions;

namespace ACore.UnitTests.Core.Services.Cache;

public class ACoreMemoryCacheDurationTests : ACoreMemoryCacheTests
{
  [Fact]
  public void DurationOkTest()
  {
    var baseDateTime = new DateTime(2020, 1, 1);
    // Arrange
    var clock = new TestSystemClock()
    {
      UtcNow = baseDateTime
    };
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey, TimeSpan.FromMinutes(10));

    var memoryCacheModuleModuleStorageAsSut = ACoreCacheAsSut(clock);

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
    clock.UtcNow = baseDateTime.AddMinutes(9);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue?.Data.Should().Be(cacheValue.Data);
  }

  [Fact]
  public void DurationExpirationTest()
  {
    var baseDateTime = new DateTime(2020, 1, 1);
    
    // Arrange
    var clock = new TestSystemClock()
    {
      UtcNow = baseDateTime
    };
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey, TimeSpan.FromMinutes(10));

    var memoryCacheModuleModuleStorageAsSut = ACoreCacheAsSut(clock);

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
    clock.UtcNow = baseDateTime.AddMinutes(11);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeFalse();
  }

}