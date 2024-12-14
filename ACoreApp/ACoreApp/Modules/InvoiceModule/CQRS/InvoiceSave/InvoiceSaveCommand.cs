using ACore.Results;
using ACoreApp.Modules.InvoiceModule.Models;

namespace ACoreApp.Modules.InvoiceModule.CQRS.InvoiceSave;

public class InvoiceSaveCommand(InvoiceItem invoiceItem) : InvoiceRequest<Result>
{
  public InvoiceItem Invoice => invoiceItem;
}