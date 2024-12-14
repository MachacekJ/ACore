using System.Reflection;
using ACore.Blazor.Configuration;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageGet;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using ACore.Blazor.Services.App.Manager;
using ACoreApp.Client.Configuration;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ACore.Client.Configuration;
using MediatR;

List<Assembly> allAssemblies = [typeof(ACoreApp.Client._Imports).Assembly, typeof(ACore.Blazor._Imports).Assembly];
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.ConfigureContainer(new AutofacServiceProviderFactory(ConfigureContainer));

builder.Services.AddOptions();
builder.Services.AddACoreAppSharedConfiguration(allAssemblies);
builder.Services.AddACoreClient();

var host = builder.Build();

var appManager = host.Services.GetRequiredService<IAppManager>();
var mediator = host.Services.GetService<IMediator>() ?? throw new NullReferenceException();
var languageFromLocalStorage = await mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.Resources, "currentLanguage"));

// var lcid = SupportedLanguage.AllSupportedLanguages.First().LCID;
// if (languageFromLocalStorage.IsValue)
//   lcid = languageFromLocalStorage.GetValue<int>();

//await appManager.GetExtension<LocalizationAppExtension>().SetStartupLanguage(lcid, mediator);

await host.RunAsync();
return;

static void ConfigureContainer(ContainerBuilder containerBuilder)
{
  containerBuilder.ContainerACoreBlazor();
}