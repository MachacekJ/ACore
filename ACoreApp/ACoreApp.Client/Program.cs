using ACore.Blazor.Configuration;
using ACoreApp.Client.Configuration;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.ConfigureContainer(new AutofacServiceProviderFactory(ConfigureContainer));

builder.Services.AddOptions();
builder.Services.AddACoreAppSharedConfiguration();

await builder.Build().RunAsync();
return;

static void ConfigureContainer(ContainerBuilder containerBuilder)
{
  containerBuilder.ContainerACoreBlazor();
}