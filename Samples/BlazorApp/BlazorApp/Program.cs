using System.Reflection;
using ACore.Blazor.Configuration;
using ACore.Modules.LocalizationModule.Repositories;
using ACore.Modules.LocalizationModule.Repositories.Implementations;
using ACore.Repository.Definitions.Models;
using ACore.Server.Repository;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlazorApp.Client.Configuration;
using BlazorApp.Configuration;
using BlazorApp.UI;

List<Assembly> allAssemblies = [typeof(BlazorApp.Client._Imports).Assembly, typeof(ACore.Blazor._Imports).Assembly];

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer));

builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();


var aCoreSettings = builder.Configuration.GetSection("ACoreServer").Get<BlazorAppSettings>() ?? throw new NullReferenceException();

// var localizationRepository = new MemoryLocalizationRepository([
//   new ACoreACoreLocalizationItem(typeof(BlazorApp.Client.Configuration.Localization.Contexts.Memory.ResXContext), "Test", 1033, "TestEn"),
//   new ACoreACoreLocalizationItem(typeof(BlazorApp.Client.Configuration.Localization.Contexts.Memory.ResXContext), "Test", 1029, "TestCZ"),
//   new ACoreACoreLocalizationItem(typeof(BlazorApp.Client.Configuration.Localization.Contexts.Memory.ResXContext2), "Test", 1033, "TestEn2"),
//   new ACoreACoreLocalizationItem(typeof(BlazorApp.Client.Configuration.Localization.Contexts.Memory.ResXContext2), "Test", 1029, "TestCZ2"),
// ]);

IEnumerable<ILocalizationRepository> localizationRepositories =
[
  new EmbeddedJsonLocalizationRepository()
  //new ResXLocalizationRepository(),

  //localizationRepository
];

// TODO different repositories for modules
builder.Services.AddBlazorApp(b =>
{
  #region ACore SERVER configuration

  b.AddDefaultRepositories(s => s
    .DefaultRepositoryType(RepositoryTypeEnum.Postgres)
    .AddMongo(aCoreSettings.Mongo.ConnectionString, aCoreSettings.Mongo.CollectionName)
    .AddPG(aCoreSettings.PG)
  );

  b.AddServerCache(c =>
  {
    c.Categories = [CacheCategories.Entity, CacheCategories.Localization];
    c.RedisOptions.ConnectionString = aCoreSettings.Redis.Server;
    c.RedisOptions.Password = aCoreSettings.Redis.Password;
    c.RedisOptions.InstanceName = aCoreSettings.Redis.InstanceName;
  });
  b.AddAuditModule(m => m.DefaultRepositoryType(RepositoryTypeEnum.Mongo));
  b.AddLocalizationModule(BlazorApp.Client.Configuration.Localization.SupportedLanguage.AllSupportedLanguages.Select(i => i.Name).ToArray(),
    l => { l.AddLocalizationRepositories(localizationRepositories); }
  );

  #endregion

  #region BlazorApp configuration

  b.AddInvoiceModule();
  b.AddCustomerModule(
    customerModuleOptionsBuilder => customerModuleOptionsBuilder.DefaultRepositoryType(RepositoryTypeEnum.Mongo)
  );

  #endregion
});

builder.Services.AddBlazorAppSharedConfiguration(allAssemblies);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

await app.UseBlazorApp();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllers();
app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode()
  .AddInteractiveWebAssemblyRenderMode()
  .AddAdditionalAssemblies(allAssemblies.ToArray());

app.Run();
return;

static void ConfigureContainer(ContainerBuilder containerBuilder)
{
  containerBuilder.ContainerACoreBlazor();
}