using System.Net.Http.Headers;
using ACore.Client.Modules.LocalizationModule.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Client.Modules.LocalizationModule.Configuration;

public static class LocalizationClientModuleServiceExtensions
{
  public static void AddLocalizationModule(this IServiceCollection services, LocalizationClientModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);
    
    services.AddHttpClient(nameof(LocalizationClient), client =>
    {
      // client.DefaultRequestHeaders.AcceptLanguage.Clear();
      //client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
      client.BaseAddress = options.BaseAddress;
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }).AddStandardResilienceHandler();
  }

  // public static Task UseLocalizationModule(this IServiceProvider provider)
  // {
  //   // var ifac = provider.GetService<IHttpClientFactory>() ?? throw new ArgumentException($"{nameof(IHttpClientFactory)} is not configured.");
  //   // var client = ifac.CreateClient(nameof(LocalizationClient));
  //   //
  //   // var dw = await client.GetFromJsonAsync<HomePageLocX>("/loc/ui/pages/homepage.json");
  // }
}