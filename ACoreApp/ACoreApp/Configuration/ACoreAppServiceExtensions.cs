using ACore.CQRS.Extensions;
using ACore.Server.Configuration;
using ACoreApp.Modules.CustomerModule.Configuration;
using ACoreApp.Modules.InvoiceModule.Configuration;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ACoreApp.Configuration;

public static class ACoreAppServiceExtensions
{
  public static void AddACoreApp(this IServiceCollection services, Action<ACoreAppOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreTestOptionsBuilder = ACoreAppOptionsBuilder.Default();
    optionsBuilder?.Invoke(aCoreTestOptionsBuilder);
    var oo = aCoreTestOptionsBuilder.Build();
    AddACoreApp(services, oo);
  }

  private static void AddACoreApp(this IServiceCollection services, ACoreAppOptions aCoreAppOptions)
  {
    //ValidateDependencyInConfiguration(aCoreTestOptions);
    services.AddACoreServer(aCoreAppOptions);

    var myOptionsInstance = Options.Create(aCoreAppOptions);
    services.AddSingleton(myOptionsInstance);
    
    // Adding CQRS from ACore.Tests assembly.
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreAppServiceExtensions));
      c.ParallelNotification();
    });
    services.AddValidatorsFromAssembly(typeof(ACoreAppServiceExtensions).Assembly, includeInternalTypes: true);

    if (aCoreAppOptions.InvoiceModuleOptions is { IsActive: true })
      services.AddInvoiceModule(aCoreAppOptions.InvoiceModuleOptions);

    if (aCoreAppOptions.CustomerModuleOptions is { IsActive: true })
      services.AddCustomerModule(aCoreAppOptions.CustomerModuleOptions);
  }

  public static async Task UseACoreApp(this IApplicationBuilder applicationBuilder)
  {
    await applicationBuilder.UseACoreServer();
    var provider = applicationBuilder.ApplicationServices;
    var opt = provider.GetService<IOptions<ACoreAppOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreServerOptions)} is not registered.");

    if (opt.InvoiceModuleOptions is { IsActive: true })
      await provider.UseInvoiceModule();

    if (opt.CustomerModuleOptions is { IsActive: true })
      await provider.UseCustomerModule();
  }
}