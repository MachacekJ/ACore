using ACore.Server.Configuration;
using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestInfrastructure.Storages;
using ACore.Tests.Server.TestInfrastructure.Storages.EF;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure;

public abstract class StorageTestsBase(StorageTypeEnum storage) : ServerTestsBase
{
  protected IStorageResolver? StorageResolver;
  private ITestStorage? TestStorage { get;  set; }
  
  protected override void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    TestStorage = storage switch
    {
      StorageTypeEnum.MemoryEF => new MemoryEFTestStorage(),
      StorageTypeEnum.Postgres => new PGTestStorage(TestData, Configuration ?? throw new ArgumentNullException()),
      StorageTypeEnum.Mongo => new MongoTestStorage(TestData, Configuration ?? throw new ArgumentNullException()),
      _ => throw new NotImplementedException(),
    };
    
    base.SetupACoreServer(builder);
    builder.AddSettingModule();
    TestStorage?.SetupACoreServer(builder);
  }

  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    TestStorage?.RegisterServices(services);
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    if (TestStorage != null) 
      await TestStorage.GetServices(sp);
    await base.GetServices(sp);
    StorageResolver = sp.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"{nameof(IStorageResolver)} not found.");
  }

  protected override async Task FinishedTestAsync()
  {
    if (TestStorage != null) await TestStorage.FinishedTestAsync();
  }
}