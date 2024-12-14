using ACore.Server.Modules.SettingsDbModule.CQRS;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Modules.SettingsDbModule.Repositories.EF.Memory;
using ACore.Server.Modules.SettingsDbModule.Repositories.EF.PG;
using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo;
using ACore.Server.Repository.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

internal static class SettingsDbModuleServiceExtensions
{
  public static void AddSettingsDbModule(this IServiceCollection services, SettingsDbModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SettingsDbModulePipelineBehavior<,>));

    services.AddDbMongoRepository<SettingsDbModuleMongoRepositoryImpl>(options);
    services.AddDbPGRepository<SettingsEfModuleSqlPGRepositoryImpl>(options);
    services.AddDbMemoryRepository<SettingsEfModuleRepositoryImpl>(options, nameof(ISettingsDbModuleRepository));
  }

  public static async Task UseSettingsDbModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<SettingsDbModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingsDbModuleOptions)} is not configured.");

    await provider.ConfigureMongoRepository<ISettingsDbModuleRepository, SettingsDbModuleMongoRepositoryImpl>(opt);
    await provider.ConfigurePGRepository<ISettingsDbModuleRepository, SettingsEfModuleSqlPGRepositoryImpl>(opt);
    await provider.ConfigureMemoryRepository<ISettingsDbModuleRepository, SettingsEfModuleRepositoryImpl>(opt);
  }
}