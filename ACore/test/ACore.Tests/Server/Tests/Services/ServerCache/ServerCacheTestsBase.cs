using ACore.Models.Cache;
using ACore.Server.Configuration;
using ACore.Server.Services.ServerCache;
using ACore.Tests.Server.TestInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Tests.Services.ServerCache;

public class ServerCacheTestsBase : ServerTestsBase
{
  private const string FakeCacheCategoryString = "CacheFakeCategory";
  protected readonly CacheCategory FakeCacheCategory = new(FakeCacheCategoryString);

  public required IServerCache ServerCache;

  protected override void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    base.SetupACoreServer(builder);
    builder.AddServerCache(opt =>
    {
      opt.Categories.Add(FakeCacheCategory);
      opt.RedisOptions.Password = "password";
      opt.RedisOptions.ConnectionString = Configuration?["TestSettings:Redis"] ?? throw new InvalidOperationException();
      opt.RedisOptions.InstanceName = nameof(ACore) + "Tests";
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    ServerCache = sp.GetService<IServerCache>() ?? throw new InvalidOperationException();
  }
}