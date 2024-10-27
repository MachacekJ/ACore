using ACore.Base.Cache;
using ACore.Configuration;
using ACore.Modules.MemoryCacheModule.Configuration;
using ACore.Modules.MemoryCacheModule.Storages;
using ACore.UnitTests.Core.Modules.MemoryCacheModule.FakeClasses;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace ACore.UnitTests.Core.Modules.MemoryCacheModule.Storages;

public class MemoryCacheModuleStorageTests
{
  private const string FakeCacheCategoryString = "CacheFakeCategory";
  private const string FakeCacheKey = "CacheFakeKey";
  private readonly CacheCategory _fakeCacheCategory = new(FakeCacheCategoryString);
  private readonly CacheCategory _fakeCacheCategory2 = new(FakeCacheCategoryString + "2");
  private const string FakeCacheSubCategoryString = "CacheFakeSubCategory";
  private readonly CacheCategory _fakeCacheSubCategory = new(FakeCacheSubCategoryString);
  private readonly CacheCategory _fakeCacheSubCategory2 = new(FakeCacheSubCategoryString + "2");

  [Fact]
  public void StringTest()
  {
    // Arrange
    var cacheValue = "Hello";
    var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey);
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<string>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue.Should().Be(cacheValue);
  }

  [Fact]
  public void ObjectTest()
  {
    // Arrange
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey);
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue?.Data.Should().Be(cacheValue.Data);
  }
  
  [Fact]
  public void ObjectSubCategoryTest()
  {
    // Arrange
    var cacheValue = new FakeCachedData { Data = "Hello" };
    var cacheKey = CacheKey.Create(_fakeCacheCategory, _fakeCacheSubCategory, FakeCacheKey);
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out var cachedValue).Should().BeTrue();
    cachedValue?.Data.Should().Be(cacheValue.Data);
  }

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
    var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey, TimeSpan.FromMinutes(10));

    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut(clock);

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
    var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey, TimeSpan.FromMinutes(10));

    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut(clock);

    // Act
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
    clock.UtcNow = baseDateTime.AddMinutes(11);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeFalse();
  }

  [Fact]
  public void RemoveTest()
  {
    // Arrange
    var cacheValue = "Hello";
    var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey);
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();
    memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);

    // Act
    memoryCacheModuleModuleStorageAsSut.Remove(cacheKey);

    // Assert
    memoryCacheModuleModuleStorageAsSut.TryGetValue<string>(cacheKey, out _).Should().BeFalse();
  }

  [Fact]
  public void RemoveCategoryStringValueTest()
  {
    const int max = 10;
    
    // Arrange
    var cacheValue = "Hello";
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    memoryCacheModuleModuleStorageAsSut.RemoveCategory(_fakeCacheCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.TryGetValue<string>(cacheKey, out _).Should().BeTrue();
      memoryCacheModuleModuleStorageAsSut.TryGetValue<string>(cacheKey2, out _).Should().BeFalse();
    }
  }

  [Fact]
  public void RemoveCategoryObjectValueTest()
  {
    const int max = 10;
    
    // Arrange
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheValue = new FakeCachedData { Data = "Hello" + i };
      var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    memoryCacheModuleModuleStorageAsSut.RemoveCategory(_fakeCacheCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(_fakeCacheCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeTrue();
      memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey2, out _).Should().BeFalse();
    }
  }
  
  [Fact]
  public void RemoveSubCategoryObjectValueTest()
  {
    const int max = 10;
    
    // Arrange
    var memoryCacheModuleModuleStorageAsSut = MemoryCacheModuleModuleStorageAsSut();
    for (var i = 0; i < max; i++)
    {
      var cacheValue = new FakeCachedData { Data = "Hello" + i };
      var cacheKey = CacheKey.Create(_fakeCacheCategory, _fakeCacheSubCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory, _fakeCacheSubCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey, cacheValue);
      memoryCacheModuleModuleStorageAsSut.Set(cacheKey2, cacheValue);
    }

    // Act
    memoryCacheModuleModuleStorageAsSut.RemoveCategory(_fakeCacheCategory, _fakeCacheSubCategory2);

    // Assert
    for (var i = 0; i < max; i++)
    {
      var cacheKey = CacheKey.Create(_fakeCacheCategory, _fakeCacheSubCategory, FakeCacheKey + i);
      var cacheKey2 = CacheKey.Create(_fakeCacheCategory, _fakeCacheSubCategory2, FakeCacheKey + i);
      memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey, out _).Should().BeTrue();
      memoryCacheModuleModuleStorageAsSut.TryGetValue<FakeCachedData>(cacheKey2, out _).Should().BeFalse();
    }
  }
  
  private IMemoryCacheModuleStorage MemoryCacheModuleModuleStorageAsSut(ISystemClock? systemClock = null)
  {
    var memoryCacheOptions = Options.Create(new MemoryCacheOptions()
    {
      Clock = systemClock
    });
    var aCoreOptions = Options.Create(new ACoreOptions()
    {
      MemoryCacheModuleOptions = new MemoryCacheModuleOptions(true)
      {
        Categories = [_fakeCacheCategory, _fakeCacheCategory2]
      }
    });
    var mc = new MemoryCache(memoryCacheOptions);
    return new MemoryCacheModuleModuleStorage(mc, aCoreOptions);
  }
}