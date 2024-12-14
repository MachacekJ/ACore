using Autofac;
using ACore.Configuration;
using ACore.Configuration.CQRS;
using ACore.CQRS.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.SecurityModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Server.Services;
using ACore.Server.Services.Security;
using ACore.Server.Services.ServerCache.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration;

public static class ACoreServerServiceExtensions
{
  public static void AddACoreServer(this IServiceCollection services, Action<ACoreServerOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreServerOptionBuilder = ACoreServerOptionsBuilder.Empty();
    optionsBuilder?.Invoke(aCoreServerOptionBuilder);
    var aCoreServerOptions = aCoreServerOptionBuilder.Build();
    AddACoreServer(services, aCoreServerOptions);
  }

  public static void AddACoreServer(this IServiceCollection services, ACoreServerOptions aCoreServerOptions)
  {
    ValidateDependencyInConfiguration(aCoreServerOptions);

    services.AddACore();

    var myOptionsInstance = Options.Create(aCoreServerOptions);
    services.AddSingleton(myOptionsInstance);

    // Adding CQRS only from ACore assembly.
    services.AddACoreMediatr();
    // Adding CQRS from ACore.Server assembly.
    services.AddMediatR(c =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerServiceExtensions));
      c.ParallelNotification();
    });
    services.AddValidatorsFromAssembly(typeof(ACoreServerServiceExtensions).Assembly, includeInternalTypes: true);

    services.AddServerCache(aCoreServerOptions.ServerCache);
    services.TryAddScoped<ISecurity, EmptySecurity>();
    services.AddScoped<IACoreServerCurrentScope, ACoreServerCurrentScope>();
    services.TryAddSingleton<IRepositoryResolver>(new DefaultRepositoryResolver());

    services.AddACoreServerModules(aCoreServerOptions);
  }

  public static async Task UseACoreServer(this IApplicationBuilder applicationBuilder)
  {
    var provider = applicationBuilder.ApplicationServices;
    var opt = provider.GetService<IOptions<ACoreServerOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreServerOptions)} is not registered.");

    if (opt.SettingsDbModuleOptions is { IsActive: true })
      await provider.UseSettingsDbModule();
    if (opt.AuditModuleOptions is { IsActive: true })
      await provider.UseAuditModule();

    if (opt.LocalizationServerModuleOptions is { IsActive: true })
      await applicationBuilder.UseLocalizationServerModule();
  }

  public static void ContainerACoreServer(this ContainerBuilder containerBuilder)
  {
    containerBuilder.ContainerAuditModule();
  }

  private static void AddACoreServerModules(this IServiceCollection services, ACoreServerOptions aCoreServerOptions)
  {
    if (aCoreServerOptions.SettingsDbModuleOptions is { IsActive: true })
      services.AddSettingsDbModule(aCoreServerOptions.SettingsDbModuleOptions);

    if (aCoreServerOptions.AuditModuleOptions is { IsActive: true })
      services.AddAuditModule(aCoreServerOptions.AuditModuleOptions);

    if (aCoreServerOptions.SecurityModuleOptions is { IsActive: true })
      services.AddSecurityModule(aCoreServerOptions.SecurityModuleOptions);

    if (aCoreServerOptions.LocalizationServerModuleOptions is { IsActive: true })
      services.AddLocalizationServerModule(aCoreServerOptions.LocalizationServerModuleOptions);
  }

  private static void ValidateDependencyInConfiguration(ACoreServerOptions aCoreServerOptions)
  {
    ValidateSettingsDbOptions(aCoreServerOptions);
    ValidateAuditModuleOptions(aCoreServerOptions);
  }

  private static void ValidateAuditModuleOptions(ACoreServerOptions aCoreServerOptions)
  {
    if (aCoreServerOptions.AuditModuleOptions is not { IsActive: true })
      return;

    if (!(aCoreServerOptions.SecurityModuleOptions is { IsActive: true }))
      throw new Exception($"Module {nameof(Modules.SecurityModule)} must be activated.");
  }

  private static void ValidateSettingsDbOptions(ACoreServerOptions aCoreServerOptions)
  {
    if (aCoreServerOptions.SettingsDbModuleOptions is { IsActive: false })
      throw new Exception($"Module {nameof(Modules.SettingsDbModule)} must be activated.");
  }
}