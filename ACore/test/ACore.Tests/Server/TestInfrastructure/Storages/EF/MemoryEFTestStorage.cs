using ACore.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure.Storages.EF;

public class MemoryEFTestStorage : ITestStorage
{
  public void SetupACoreServer(ACoreServerOptionBuilder builder)
  {
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
  }

  public void RegisterServices(ServiceCollection sc) { }
  public Task GetServices(IServiceProvider sp) => Task.CompletedTask;
  public Task FinishedTestAsync() => Task.CompletedTask;
}