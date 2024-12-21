using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Configuration;
using ACore.Tests.Server.TestInfrastructure.Storages;
using ACore.Tests.Server.TestInfrastructure.Storages.EF;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure;

public abstract class StorageTestsBase(IEnumerable<StorageTypeEnum> storages) : ServerTestsBase
{
  protected IStorageResolver? StorageResolver;
  private List<ITestStorage> TestStorages { get; set; } = new();

  protected override void SetupACoreTest(ACoreTestOptionsBuilder builder)
  {
    base.SetupACoreTest(builder);
    foreach (var storage in storages)
    {
      ITestStorage testStorage = storage switch
      {
        StorageTypeEnum.MemoryEF => new MemoryEFTestStorage(),
        StorageTypeEnum.Postgres => new PGTestStorage(TestData, Configuration ?? throw new ArgumentNullException()),
        StorageTypeEnum.Mongo => new MongoTestStorage(TestData, Configuration ?? throw new ArgumentNullException()),
        _ => throw new NotImplementedException(),
      };
      TestStorages.Add(testStorage);
    }

    TestStorages.ForEach(ts => ts.ConfigureStorage(builder));
  }

  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    TestStorages.ForEach(ts => ts.RegisterServices(services));
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    foreach (var ts in TestStorages)
      await ts.GetServices(sp);

    await base.GetServices(sp);
    StorageResolver = sp.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"{nameof(IStorageResolver)} not found.");
  }

  protected override async Task FinishedTestAsync()
  {
    foreach (var ts in TestStorages)
      await ts.FinishedTestAsync();
  }
}