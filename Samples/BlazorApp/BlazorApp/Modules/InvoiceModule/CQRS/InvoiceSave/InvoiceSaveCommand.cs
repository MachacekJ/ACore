using ACore.Results;
using BlazorApp.Modules.InvoiceModule.Models;

namespace BlazorApp.Modules.InvoiceModule.CQRS.InvoiceSave;

public class InvoiceSaveCommand(InvoiceItem invoiceItem) : InvoiceRequest<Result>
{
  public InvoiceItem Invoice => invoiceItem;
}