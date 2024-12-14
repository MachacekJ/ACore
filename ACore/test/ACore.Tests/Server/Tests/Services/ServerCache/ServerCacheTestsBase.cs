using ACore.Models.Cache;
using ACore.Server.Services.ServerCache;
using ACore.Tests.Base;
using ACore.Tests.Server.FakeApp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Tests.Services.ServerCache;

public class ServerCacheTestsBase : TestsBase
{
  private const string FakeCacheCategoryString = "CacheFakeCategory";
  protected readonly CacheCategory FakeCacheCategory = new(FakeCacheCategoryString);

  public required IServerCache ServerCache;

  private void ConfigurationTestStorage(FakeAppOptionsBuilder builder)
  {
    builder.AddServerCache(serverCacheOptions =>
    {
      serverCacheOptions.Categories.Add(FakeCacheCategory);
      serverCacheOptions.RedisOptions.Password = Configuration?["TestSettings:RedisPassword"] ?? throw new InvalidOperationException();
      serverCacheOptions.RedisOptions.ConnectionString = Configuration?["TestSettings:Redis"] ?? throw new InvalidOperationException();
      serverCacheOptions.RedisOptions.InstanceName = nameof(ACore) + "Tests";
    });
  }

  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    services.AddFakeApp(ConfigurationTestStorage);
  }

  protected override async Task<IServiceProvider> UseServices(IApplicationBuilder appBuilder)
  {
    var sp = await base.UseServices(appBuilder);
    ServerCache = sp.GetService<IServerCache>() ?? throw new InvalidOperationException();
    return await Task.FromResult(sp);
  }
}