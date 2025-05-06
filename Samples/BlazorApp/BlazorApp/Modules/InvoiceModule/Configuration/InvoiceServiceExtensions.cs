using ACore.Server.Repository.Configuration;
using BlazorApp.Modules.InvoiceModule.CQRS;
using BlazorApp.Modules.InvoiceModule.Repository;
using BlazorApp.Modules.InvoiceModule.Repository.EF.PG;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace BlazorApp.Modules.InvoiceModule.Configuration;

public static class InvoiceServiceExtensions
{
  public static void AddInvoiceModule(this IServiceCollection services, InvoiceModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(InvoicePipelineBehavior<,>));
    
    services.AddDbPGRepository<InvoicePGRepositoryImpl>(options);
  }

  public static async Task UseInvoiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<InvoiceModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(InvoiceModuleOptions)} is not configured.");
    
    await provider.ConfigurePGRepository<IInvoiceRepository, InvoicePGRepositoryImpl>(opt);
  }
}