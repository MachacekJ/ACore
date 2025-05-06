using ACore.Server.Configuration;
using BlazorApp.Modules.CustomerModule.Configuration;
using BlazorApp.Modules.InvoiceModule.Configuration;

namespace BlazorApp.Configuration;

public class BlazorAppOptions : ACoreServerOptions
{
  public InvoiceModuleOptions? InvoiceModuleOptions { get; set; }
  public CustomerModuleOptions? CustomerModuleOptions { get; set; }
}