using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Blazor.Services.HttpClients.Configuration;

public static class ACoreHttpClientServiceExtensions
{
  public static void AddACoreHttpClients(this IServiceCollection services, ACoreHttpClientOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.AddSingleton(myOptionsInstance);

 
      
    // services.AddHttpClient(AntiforgeryHttpClientFactory.AuthorizedClientName, client =>
    // {
    //   client.DefaultRequestHeaders.AcceptLanguage.Clear();
    //   client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
    //   client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    //   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    // }).AddHttpMessageHandler<AuthorizedHandler>();
  }
}