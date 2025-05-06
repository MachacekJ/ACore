using ACore.Server.Configuration;
using BlazorApp.Modules.CustomerModule.Configuration;
using BlazorApp.Modules.InvoiceModule.Configuration;

namespace BlazorApp.Configuration;

public class BlazorAppOptionsBuilder : ACoreServerOptionsBuilder
{
  private readonly InvoiceModuleOptionsBuilder _invoiceModuleOptionBuilder = InvoiceModuleOptionsBuilder.Default();
  private readonly CustomerModuleOptionsBuilder _customerModuleOptionBuilder = CustomerModuleOptionsBuilder.Default();

  public static BlazorAppOptionsBuilder Default() => new();

  public void AddInvoiceModule(Action<InvoiceModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_invoiceModuleOptionBuilder);
    _invoiceModuleOptionBuilder.Activate();
  }
  
  public void AddCustomerModule(Action<CustomerModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_customerModuleOptionBuilder);
    _customerModuleOptionBuilder.Activate();
  }
  
  public override BlazorAppOptions Build()
  {
    var res = new BlazorAppOptions();
    SetOptions(res);
    return res;
  }

  private void SetOptions(BlazorAppOptions opt)
  {
    base.SetOptions(opt);
    opt.InvoiceModuleOptions = _invoiceModuleOptionBuilder.Build(DefaultRepositoryOptionBuilder);
    opt.CustomerModuleOptions = _customerModuleOptionBuilder.Build(DefaultRepositoryOptionBuilder);
  }
}