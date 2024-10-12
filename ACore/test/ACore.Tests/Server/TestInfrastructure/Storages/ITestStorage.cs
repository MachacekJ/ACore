using ACore.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure.Storages;

public interface ITestStorage
{
  void SetupACoreServer(ACoreServerOptionBuilder builder);
  void RegisterServices(ServiceCollection sc);
  Task GetServices(IServiceProvider sp);
  Task FinishedTestAsync();
}