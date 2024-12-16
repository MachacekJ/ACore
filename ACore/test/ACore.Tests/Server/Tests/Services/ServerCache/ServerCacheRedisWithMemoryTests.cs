using System.Reflection;
using ACore.Models.Cache;
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
      // Arrange
      var cacheKey = CacheKey.Create(FakeCacheCategory, FakeCacheKey);
      // Act
      await ServerCache.Set(cacheKey, "test");
      var a = await ServerCache.Get<string>(cacheKey);
      await ServerCache.Remove(cacheKey);
      var a2 = await ServerCache.Get<string>(cacheKey);
      
      await ServerCache.Set(cacheKey, "test");
      await ServerCache.RemoveCategory(FakeCacheCategory);
      var a3 = await ServerCache.Get<string>(cacheKey);
    });
  }
}