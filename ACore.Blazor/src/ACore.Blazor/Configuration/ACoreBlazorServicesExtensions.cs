using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App.Manager;
using ACore.Blazor.Services.HttpClients.Configuration;
using ACore.Configuration;
using Autofac;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Blazor.Configuration;

public static class ACoreBlazorServicesExtensions
{
  public static void AddACoreBlazor(this IServiceCollection services, Action<ACoreBlazorOptionsBuilder>? optionsBuilder = null)
  {
    var builder = new ACoreBlazorOptionsBuilder();
    optionsBuilder?.Invoke(builder);
    var aCoreBlazorOptions = builder.Build();
    AddACoreBlazor(services, aCoreBlazorOptions);
  }

  private static void AddACoreBlazor(this IServiceCollection services, ACoreBlazorOptions options)
  {
    services.AddACore();

    services.AddTelerikBlazor();
    services.AddLocalization();


    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(ACoreBlazorServicesExtensions)); });
    services.AddValidatorsFromAssembly(typeof(ACoreBlazorServicesExtensions).Assembly, includeInternalTypes: true);

    services.AddLocalStorageModule(options.LocalStorage);

    services.AddSingleton(options.AppPages);
    services.AddSingleton<IAppManager, AppManager>();

    if (options.BaseAddress != null)
      services.AddACoreHttpClients(new ACoreHttpClientOptions
      {
        BaseAddress = options.BaseAddress
      });
  }


  public static void ContainerACoreBlazor(this ContainerBuilder containerBuilder)
  {
    containerBuilder.ContainerACore();
  }
}