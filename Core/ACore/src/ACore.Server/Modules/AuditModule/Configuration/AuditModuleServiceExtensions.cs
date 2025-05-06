using ACore.Server.Modules.AuditModule.CQRS;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.Repositories;
using ACore.Server.Modules.AuditModule.Repositories.EF.Memory;
using ACore.Server.Modules.AuditModule.Repositories.EF.PG;
using ACore.Server.Modules.AuditModule.Repositories.Mongo;
using ACore.Server.Repository.Configuration;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.Configuration;

internal static class AuditModuleServiceExtensions
{
  public static void AddAuditModule(this IServiceCollection services, AuditModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditModulePipelineBehavior<,>));
    
    services.AddDbMongoRepository<AuditMongoRepositoryImpl>(options);
    services.AddDbPGRepository<AuditPGRepositoryImpl>(options);
    services.AddDbMemoryRepository<AuditSqlMemoryRepositoryImpl>(options, nameof(IAuditRepository));
  }

  public static async Task UseAuditModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<AuditModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(AuditModuleOptions)} is not configured.");

    await provider.ConfigureMongoRepository<IAuditRepository, AuditMongoRepositoryImpl>(opt);
    await provider.ConfigurePGRepository<IAuditRepository, AuditPGRepositoryImpl>(opt);
    await provider.ConfigureMemoryRepository<IAuditRepository, AuditSqlMemoryRepositoryImpl>(opt);
  }

  public static void ContainerAuditModule(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }
}