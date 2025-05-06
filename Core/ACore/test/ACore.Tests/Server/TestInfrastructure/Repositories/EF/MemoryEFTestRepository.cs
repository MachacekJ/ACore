using ACore.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure.Repositories.EF;

public class MemoryEFTestRepository : ITestRepository
{
  public void SetupBuilder(ACoreServerOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
  }

  public void RegisterServices(ServiceCollection sc) { }
  public Task CreateDb(IServiceProvider sp) => Task.CompletedTask;
  public Task FinishedTestAsync() => Task.CompletedTask;
}