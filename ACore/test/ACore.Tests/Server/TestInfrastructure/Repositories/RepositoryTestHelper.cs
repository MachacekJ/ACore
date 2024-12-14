using ACore.Repository.Definitions.Models;
using ACore.Server.Configuration;
using ACore.Tests.Base.Models;
using ACore.Tests.Server.TestInfrastructure.Repositories.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure.Repositories;

public class RepositoryTestHelper
{
  private List<ITestRepository> TestRepositories { get; } = [];

  public RepositoryTestHelper(IEnumerable<RepositoryTypeEnum> repositoriesToRegister, TestData testData, IConfigurationRoot config)
  {
    foreach (var storage in repositoriesToRegister)
    {
      ITestRepository testRepository = storage switch
      {
        RepositoryTypeEnum.MemoryEF => new MemoryEFTestRepository(),
        RepositoryTypeEnum.Postgres => new PGTestRepository(testData, config ?? throw new ArgumentNullException()),
        RepositoryTypeEnum.Mongo => new MongoTestRepository(testData, config ?? throw new ArgumentNullException()),
        _ => throw new NotImplementedException(),
      };
      TestRepositories.Add(testRepository);
    }
  }

  public void SetupBuilder(ACoreServerOptionsBuilder builder)
    => TestRepositories.ForEach(ts => ts.SetupBuilder(builder));

  public void RegisterServices(ServiceCollection services)
    => TestRepositories.ForEach(ts => ts.RegisterServices(services));

  public async Task CreateTestStorage(IServiceProvider sp)
  {
    foreach (var ts in TestRepositories)
      await ts.CreateDb(sp);
  }

  public async Task RemoveTestStorage()
  {
    foreach (var ts in TestRepositories)
      await ts.FinishedTestAsync();
  }
}