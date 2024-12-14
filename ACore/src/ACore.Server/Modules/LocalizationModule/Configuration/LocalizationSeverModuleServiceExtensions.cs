using ACore.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.LocalizationModule.CQRS;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.LocalizationModule.Configuration;

internal static class LocalizationSeverModuleServiceExtensions
{
  public static void AddLocalizationServerModule(this IServiceCollection services, LocalizationServerModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LocalizationModulePipelineBehavior<,>));
    services.AddLocalizationModule(options.LocalizationModuleOptions);
  }

  public static Task UseLocalizationServerModule(this IApplicationBuilder applicationBuilder)
  {
    var provider = applicationBuilder.ApplicationServices;
    var opt = provider.GetService<IOptions<LocalizationServerModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(LocalizationServerModuleOptions)} is not configured.");


    
    var allSupportedLanguages = opt.LocalizationModuleOptions.SupportedLanguages.ToArray();
    applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions()
      .SetDefaultCulture(allSupportedLanguages.First())
      .AddSupportedCultures(allSupportedLanguages)
      .AddSupportedUICultures(allSupportedLanguages));

    return Task.CompletedTask;
  }
}