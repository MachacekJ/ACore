using ACore.Server.Configuration;
using ACoreApp.Modules.CustomerModule.Configuration;
using ACoreApp.Modules.InvoiceModule.Configuration;

namespace ACoreApp.Configuration;

public class ACoreAppOptionsBuilder : ACoreServerOptionsBuilder
{
  private readonly InvoiceModuleOptionsBuilder _invoiceModuleOptionBuilder = InvoiceModuleOptionsBuilder.Default();
  private readonly CustomerModuleOptionsBuilder _customerModuleOptionBuilder = CustomerModuleOptionsBuilder.Default();

  public static ACoreAppOptionsBuilder Default() => new();

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
  
  public override ACoreAppOptions Build()
  {
    var res = new ACoreAppOptions();
    SetOptions(res);
    return res;
  }

  private void SetOptions(ACoreAppOptions opt)
  {
    base.SetOptions(opt);
    opt.InvoiceModuleOptions = _invoiceModuleOptionBuilder.Build(DefaultRepositoryOptionBuilder);
    opt.CustomerModuleOptions = _customerModuleOptionBuilder.Build(DefaultRepositoryOptionBuilder);
  }
}