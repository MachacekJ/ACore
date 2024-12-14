using ACoreApp.Modules.CustomerModule.CQRS;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACoreApp.Modules.CustomerModule.Configuration;

public static class CustomerServiceExtensions
{
  public static void AddCustomerModule(this IServiceCollection services, CustomerModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CustomerPipelineBehavior<,>));
   // services.AddDbMongoRepository<CustomerMongoRepositoryImp>(options);
    
  }

  public static async Task UseCustomerModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<CustomerModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(CustomerModuleOptions)} is not configured.");
    
    //await provider.ConfigureMongoRepository<ICustomerRepository, CustomerMongoRepositoryImp>(opt);
  }
}