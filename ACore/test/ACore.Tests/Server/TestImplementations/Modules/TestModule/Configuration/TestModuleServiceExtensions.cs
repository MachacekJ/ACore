using ACore.Server.Storages.Configuration;
using ACore.Tests.Server.TestImplementations.Configuration;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Delete;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Get;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Get;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Get;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Save;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Memory;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.PG;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

public static class TestModuleServiceExtensions
{
  public static void AddTestModule(this IServiceCollection services, TestModuleOptions options)
  {
   // services.AddMediatR(c => { c.RegisterServicesFromAssemblyContaining(typeof(ITestStorageModule)); });
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(TestModulePipelineBehavior<,>));
    
    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    
    services.AddDbMongoStorage<TestModuleMongoRepositoryImpl>(options.Storages);
    services.AddDbPGStorage<TestModulePGRepositoryImpl>(options.Storages);
    services.AddDbMemoryStorage<TestModuleMemoryRepositoryImpl>(options.Storages, nameof(ITestRepositoryModule));
    
    TestNoAuditData.MapConfig();
  }

  public static void ConfigureAutofacTestModule(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(TestAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAuditDeleteHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestNoAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestNoAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestValueTypeGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestValueTypeSaveHashHandler<>)).AsImplementedInterfaces();
  }

  public static async Task UseTestModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreTestOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(TestModuleOptions)} is not configured.");

    StorageOptions? storageOptions = null;
    if (opt.DefaultStorages != null)
      storageOptions = opt.DefaultStorages;
    if (opt.TestModuleOptions.Storages != null)
      storageOptions = opt.TestModuleOptions.Storages;
    if (storageOptions == null)
      throw new ArgumentException($"{nameof(opt.TestModuleOptions)} is null. You can also use {nameof(opt.DefaultStorages)}.");


    await provider.ConfigureMongoStorage<ITestRepositoryModule, TestModuleMongoRepositoryImpl>(storageOptions);
    await provider.ConfigurePGStorage<ITestRepositoryModule, TestModulePGRepositoryImpl>(storageOptions);
    await provider.ConfigureMemoryStorage<ITestRepositoryModule, TestModuleMemoryRepositoryImpl>(storageOptions);
  }
}