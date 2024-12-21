using System.Reflection;
using ACore.Models.Cache;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Server.Tests.Services.ServerCache;

public class ServerCacheRedisWithMemoryTests : ServerCacheTestsBase
{
  private const string FakeCacheKey = "CacheFakeKey";


  [Fact]
  public async Task BasicTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var cacheValue = "test";
      // Arrange
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey);
     
      // Act - Set
      await ServerCache.Set(cacheKey, cacheValue);
      var result1 = await ServerCache.Get<string>(cacheKey);
      result1.Should().Be(cacheValue);
      
      // Act - Remove
      await ServerCache.Remove(cacheKey);
      var result2 = await ServerCache.Get<string>(cacheKey);
      result2.Should().BeNull();
      
      // Act - Remove category
      await ServerCache.Set(cacheKey, "test");
      await ServerCache.RemoveCategory(FakeCacheCategory);
      var result3 = await ServerCache.Get<string>(cacheKey);
      result3.Should().BeNull();
      
    });
  }
}