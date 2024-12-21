using ACore.Server.Modules.AuditModule.CQRS;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.Repositories;
using ACore.Server.Modules.AuditModule.Repositories.Mongo;
using ACore.Server.Modules.AuditModule.Repositories.SQL.Memory;
using ACore.Server.Modules.AuditModule.Repositories.SQL.PG;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages.Configuration;
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
    
    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");

    services.AddDbMongoStorage<AuditMongoRepositoryImpl>(options.Storages);
    services.AddDbPGStorage<AuditPGEFRepositoryImpl>(options.Storages);
    services.AddDbMemoryStorage<AuditSqlMemoryRepositoryImpl>(options.Storages, nameof(IAuditRepository));
  }

  public static async Task UseAuditServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<AuditModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingsDbModuleOptions)} is not configured.");

    if (opt.Storages == null)
      throw new ArgumentException($"{nameof(opt.Storages)} is null.");

    await provider.ConfigureMongoStorage<IAuditRepository, AuditMongoRepositoryImpl>(opt.Storages);
    await provider.ConfigurePGStorage<IAuditRepository, AuditPGEFRepositoryImpl>(opt.Storages);
    await provider.ConfigureMemoryStorage<IAuditRepository, AuditSqlMemoryRepositoryImpl>(opt.Storages);
  }

  public static void ContainerAuditModule(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }
}