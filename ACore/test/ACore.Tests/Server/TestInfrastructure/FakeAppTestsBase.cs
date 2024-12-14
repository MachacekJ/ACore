using ACore.Repository.Definitions.Models;
using ACore.Server.Configuration;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Base;
using ACore.Tests.Server.FakeApp.Configuration;
using ACore.Tests.Server.TestInfrastructure.Repositories;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.TestInfrastructure;

public abstract class FakeAppTestsBase(IEnumerable<RepositoryTypeEnum> repositories) : TestsBase
{
  protected IRepositoryResolver? StorageResolver;
  private RepositoryTestHelper? _storageTestHelper;

  protected virtual void SetupBuilder(FakeAppOptionsBuilder builder)
  {
    _storageTestHelper?.SetupBuilder(builder);
  }

  protected override void RegisterServices(ServiceCollection services)
  {
    base.RegisterServices(services);
    _storageTestHelper = new RepositoryTestHelper(repositories, TestData, Configuration ?? throw new ArgumentNullException());
    services.AddFakeApp(SetupBuilder);
    _storageTestHelper?.RegisterServices(services);
  }

  protected override async Task<IServiceProvider> UseServices(IApplicationBuilder appBuilder)
  {
    var sp = await base.UseServices(appBuilder);
    if (_storageTestHelper == null)
      throw new ArgumentException();
    
    await _storageTestHelper.CreateTestStorage(sp);
    await appBuilder.UseFakeApp();
    
    StorageResolver = sp.GetService<IRepositoryResolver>() ?? throw new ArgumentNullException($"{nameof(IRepositoryResolver)} not found.");
    return await Task.FromResult(sp);
  }

  protected override void SetAutofacContainer(ContainerBuilder containerBuilder)
  {
    base.SetAutofacContainer(containerBuilder);
    containerBuilder.ContainerACoreServer();
    containerBuilder.RegisterFakeAppContainer();
  }

  protected override async Task ClearTestAsync()
  {
    if (_storageTestHelper == null)
      throw new ArgumentException();
    await _storageTestHelper.RemoveTestStorage();
  }
}