// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using Microsoft.Extensions.Options;
//
// namespace ACore.Services.Localization.Configuration;
//
// public static class ACoreLocalizationExtensions
// {
//   public static IServiceCollection AddACoreLocalization(
//     this IServiceCollection services,
//     ACoreLocalizationOptions? options = null)
//   {
//     var localizationOptions = options ?? new ACoreLocalizationOptions();
//
//     var myOptionsInstance = Options.Create(localizationOptions);
//     services.TryAddSingleton(myOptionsInstance);
//     
//
//     return services;
//   }
// }