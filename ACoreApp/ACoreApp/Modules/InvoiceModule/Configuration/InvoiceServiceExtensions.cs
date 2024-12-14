using ACore.Server.Repository.Configuration;
using ACoreApp.Modules.InvoiceModule.CQRS;
using ACoreApp.Modules.InvoiceModule.Repository;
using ACoreApp.Modules.InvoiceModule.Repository.EF.PG;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACoreApp.Modules.InvoiceModule.Configuration;

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