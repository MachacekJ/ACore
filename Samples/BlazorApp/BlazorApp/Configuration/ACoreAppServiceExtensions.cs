using ACore.CQRS.Extensions;
using ACore.Server.Configuration;
using BlazorApp.Modules.CustomerModule.Configuration;
using BlazorApp.Modules.InvoiceModule.Configuration;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace BlazorApp.Configuration;

public static class BlazorAppServiceExtensions
{
  public static void AddBlazorApp(this IServiceCollection services, Action<BlazorAppOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreTestOptionsBuilder = BlazorAppOptionsBuilder.Default();
    optionsBuilder?.Invoke(aCoreTestOptionsBuilder);
    var oo = aCoreTestOptionsBuilder.Build();
    AddBlazorApp(services, oo);
  }

  private static void AddBlazorApp(this IServiceCollection services, BlazorAppOptions BlazorAppOptions)
  {
    //ValidateDependencyInConfiguration(aCoreTestOptions);
    services.AddACoreServer(BlazorAppOptions);

    var myOptionsInstance = Options.Create(BlazorAppOptions);
    services.AddSingleton(myOptionsInstance);
    
    // Adding CQRS from ACore.Tests assembly.
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(BlazorAppServiceExtensions));
      c.ParallelNotification();
    });
    services.AddValidatorsFromAssembly(typeof(BlazorAppServiceExtensions).Assembly, includeInternalTypes: true);

    if (BlazorAppOptions.InvoiceModuleOptions is { IsActive: true })
      services.AddInvoiceModule(BlazorAppOptions.InvoiceModuleOptions);

    if (BlazorAppOptions.CustomerModuleOptions is { IsActive: true })
      services.AddCustomerModule(BlazorAppOptions.CustomerModuleOptions);
  }

  public static async Task UseBlazorApp(this IApplicationBuilder applicationBuilder)
  {
    await applicationBuilder.UseACoreServer();
    var provider = applicationBuilder.ApplicationServices;
    var opt = provider.GetService<IOptions<BlazorAppOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreServerOptions)} is not registered.");

    if (opt.InvoiceModuleOptions is { IsActive: true })
      await provider.UseInvoiceModule();

    if (opt.CustomerModuleOptions is { IsActive: true })
      await provider.UseCustomerModule();
  }
}