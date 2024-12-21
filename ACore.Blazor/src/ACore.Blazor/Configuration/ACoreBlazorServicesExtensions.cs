using ACore.Blazor.Modules.LocalStorageModule.Configuration;
using ACore.Blazor.Services.App;
using ACore.Blazor.Services.Page.Implementations;
using ACore.Blazor.Services.Page.Interfaces;
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
    services.AddACore(options);

    services.AddTelerikBlazor();
    services.AddLocalization();

    
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(ACoreBlazorServicesExtensions)); });
    services.AddValidatorsFromAssembly(typeof(ACoreBlazorServicesExtensions).Assembly, includeInternalTypes: true);

    services.AddLocalStorageModule(options.LocalStorage);
    
    services.AddSingleton(options.AppPages);
    services.AddSingleton<IAppState, AppState>();
    services.AddSingleton<IPageManager, PageManager>();
  }

  public static void ContainerACoreBlazor(this ContainerBuilder containerBuilder)
  {
    containerBuilder.ContainerACore();
  }
}