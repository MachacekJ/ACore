using ACore.Services.Localization.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ACore.Modules.LocalizationModule.Configuration;

public static class LocalizationModuleServiceExtensions
{
  public static IServiceCollection AddLocalizationModule(
    this IServiceCollection services,
    LocalizationModuleOptions? options = null)
  {
    var localizationOptions = options ?? new LocalizationModuleOptions();
    var myOptionsInstance = Options.Create(localizationOptions);
    services.TryAddSingleton(myOptionsInstance);
    
    services.TryAddSingleton<IStringLocalizerFactory, ACoreStringLocalizerFactory>();
    services.TryAddSingleton<IStringLocalizer, ACoreStringLocalizer>();
    services.AddLocalization();
    
    return services;
  }
}