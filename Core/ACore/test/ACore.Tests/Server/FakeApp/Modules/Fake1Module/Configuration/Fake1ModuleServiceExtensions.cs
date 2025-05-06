using ACore.Server.Repository.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Delete;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Memory;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;

public static class Fake1ModuleServiceExtensions
{
  public static void AddTestModule(this IServiceCollection services, Fake1ModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(Fake1PipelineBehavior<,>));


    services.AddDbMongoRepository<Fake1MongoRepositoryImpl>(options);
    services.AddDbPGRepository<Fake1PGRepositoryImpl>(options);
    services.AddDbMemoryRepository<Fake1MemoryRepositoryImpl>(options, nameof(IFake1Repository));

    TestNoAuditData.MapsterConfig();
  }

  public static void ContainerTestModule(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(Fake1AuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1AuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1AuditDeleteHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1NoAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1NoAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1ValueTypeGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(Fake1ValueTypeSaveHashHandler<>)).AsImplementedInterfaces();
  }

  public static async Task UseTestModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<Fake1ModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(Fake1ModuleOptions)} is not configured.");

    await provider.ConfigureMongoRepository<IFake1Repository, Fake1MongoRepositoryImpl>(opt);
    await provider.ConfigurePGRepository<IFake1Repository, Fake1PGRepositoryImpl>(opt);
    await provider.ConfigureMemoryRepository<IFake1Repository, Fake1MemoryRepositoryImpl>(opt);
  }
}