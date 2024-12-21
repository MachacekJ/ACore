using ACore.Blazor.Configuration;
using ACoreApp.Client.Configuration;
using ACoreApp.Components;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer));

builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();

builder.Services.AddACoreAppSharedConfiguration();

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

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode()
  .AddInteractiveWebAssemblyRenderMode()
  .AddAdditionalAssemblies(typeof(ACoreApp.Client._Imports).Assembly)
  .AddAdditionalAssemblies(typeof(ACore.Blazor._Imports).Assembly);

app.Run();
return;

static void ConfigureContainer(ContainerBuilder containerBuilder)
{
  containerBuilder.ContainerACoreBlazor();
}