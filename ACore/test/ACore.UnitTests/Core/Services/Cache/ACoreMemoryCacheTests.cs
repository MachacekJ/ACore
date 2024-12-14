using ACore.Models.Cache;
using ACore.Services.Cache;
using ACore.Services.Cache.Configuration;
using ACore.Services.Cache.Implementations;
using ACore.UnitTests.Core.Services.Cache.FakeClasses;
using FluentAssertions;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace ACore.UnitTests.Core.Services.Cache;

public class ACoreMemoryCacheTests
{
  protected const string FakeCacheKey = "CacheFakeKey";
  private const string FakeCacheCategoryString = "CacheFakeCategory";
  private const string FakeCacheSubCategoryString = "CacheFakeSubCategory";
  private readonly CacheCategory _unknownFakeCacheCategory = new("UnknownCacheCategory");
  protected readonly CacheCategory FakeCacheCategory = new(FakeCacheCategoryString);
  protected readonly CacheCategory FakeCacheCategory2 = new(FakeCacheCategoryString + "2");
  protected readonly CacheCategory FakeCacheSubCategory = new(FakeCacheSubCategoryString);
  protected readonly CacheCategory FakeCacheSubCategory2 = new(FakeCacheSubCategoryString + "2");

  [Fact]
  public void StringTest()
  {
    // Arrange
    var cacheValue = "Hello";
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey);
    var aCoreCacheAsSut = ACoreCacheAsSut();

    // Act
    aCoreCacheAsSut.Set(cacheKey, cacheValue);

    // Assert
    aCoreCacheAsSut.TryGetValue<string>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue.Should().Be(cacheValue);
  }

  [Fact]
  public void ObjectTest()
  {
    // Arrange
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey);
    var aCoreCacheAsSut = ACoreCacheAsSut();

    // Act
    aCoreCacheAsSut.Set(cacheKey, cacheValue);

    // Assert
    aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue?.Data.Should().Be(cacheValue.Data);
  }

  [Fact]
  public void ObjectSubCategoryTest()
  {
    // Arrange
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheSubCategory, FakeCacheKey);
    var aCoreCacheAsSut = ACoreCacheAsSut();

    // Act
    aCoreCacheAsSut.Set(cacheKey, cacheValue);

    // Assert
    aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue?.Data.Should().Be(cacheValue.Data);
  }

  [Fact]
  public void RemoveTest()
  {
    // Arrange
    var cacheValue = "Hello";
    var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey);
    var aCoreCacheAsSut = ACoreCacheAsSut();
    aCoreCacheAsSut.Set(cacheKey, cacheValue);

    // Act
    aCoreCacheAsSut.Remove(cacheKey);

    // Assert
    aCoreCacheAsSut.TryGetValue<string>(cacheKey, out _).Should().BeFalse();
  }

  [Fact]
  public void SaveCacheWithUnknownCacheCategory()
  {
    // Arrange
    var cacheValue = "Hello";
    var cacheKey = CacheKey.Create(_unknownFakeCacheCategory, FakeCacheKey);
    var aCoreCacheAsSut = ACoreCacheAsSut();

    // Act
    var exceptionTest = () => aCoreCacheAsSut.Set(cacheKey, cacheValue);

    // Assert
    exceptionTest.Should().Throw<ArgumentException>();
  }

  protected IACoreCache ACoreCacheAsSut(ISystemClock? systemClock = null)
  {
    var memoryCacheOptions = Options.Create(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()
    {
      Clock = systemClock
    });
    var aCoreOptions = Options.Create(
      new ACoreCacheOptions
      {
        Categories = [FakeCacheCategory, FakeCacheCategory2]
      });
    var mc = new Microsoft.Extensions.Caching.Memory.MemoryCache(memoryCacheOptions);
    return new ACoreMemoryCache(mc, aCoreOptions);
  }
}