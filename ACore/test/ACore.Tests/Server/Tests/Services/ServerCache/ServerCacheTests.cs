using System.Reflection;
using ACore.Models.Cache;
using Xunit;

namespace ACore.Tests.Server.Tests.Services.ServerCache;

public class ServerCacheTests : ServerCacheTestsBase
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
    });
  }
}