using ACore.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure.Repositories;

public interface ITestRepository
{
  void SetupBuilder(ACoreServerOptionsBuilder builder);
  void RegisterServices(ServiceCollection sc);
  Task CreateDb(IServiceProvider sp);
  Task FinishedTestAsync();
}