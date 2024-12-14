using ACore.Server.Configuration;
using ACoreApp.Modules.CustomerModule.Configuration;
using ACoreApp.Modules.InvoiceModule.Configuration;

namespace ACoreApp.Configuration;

public class ACoreAppOptions : ACoreServerOptions
{
  public InvoiceModuleOptions? InvoiceModuleOptions { get; set; }
  public CustomerModuleOptions? CustomerModuleOptions { get; set; }
}