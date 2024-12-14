using ACore.Models.Cache;
using ACore.UnitTests.Core.Services.Cache.FakeClasses;
using FluentAssertions;

namespace ACore.UnitTests.Core.Services.Cache;

public class ACoreMemoryCacheCategoriesTests : ACoreMemoryCacheTests
{
  [Fact]
  public void RemoveCategoryStringValueTest()
  {
    const int max = 10;

    // Arrange
    var cacheValue = "Hello";
    var aCoreCacheAsSut = ACoreCacheAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.Set(cacheKey, cacheValue);
      aCoreCacheAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    aCoreCacheAsSut.RemoveCategory(FakeCacheCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.TryGetValue<string>(cacheKey, out _).Should().BeTrue();
      aCoreCacheAsSut.TryGetValue<string>(cacheKey2, out _).Should().BeFalse();
    }
  }

  [Fact]
  public void RemoveCategoryObjectValueTest()
  {
    const int max = 10;

    // Arrange
    var aCoreCacheAsSut = ACoreCacheAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheValue = new FakeCachedData { Data = "Hello" + i };
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.Set(cacheKey, cacheValue);
      aCoreCacheAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    aCoreCacheAsSut.RemoveCategory(FakeCacheCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeTrue();
      aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey2, out _).Should().BeFalse();
    }
  }

  [Fact]
  public void RemoveSubCategoryObjectValueTest()
  {
    const int max = 10;

    // Arrange
    var aCoreCacheAsSut = ACoreCacheAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheValue = new FakeCachedData { Data = "Hello" + i };
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheSubCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory, FakeCacheSubCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.Set(cacheKey, cacheValue);
      aCoreCacheAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    aCoreCacheAsSut.RemoveCategory(FakeCacheCategory, FakeCacheSubCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheSubCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(FakeCacheCategory, FakeCacheSubCategory2, FakeCacheKey + i);
      aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeTrue();
      aCoreCacheAsSut.TryGetValue<FakeCachedData>(cacheKey2, out _).Should().BeFalse();
    }
  }
}