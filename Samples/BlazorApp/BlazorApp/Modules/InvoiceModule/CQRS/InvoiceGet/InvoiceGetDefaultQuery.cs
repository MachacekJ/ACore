using ACore.Results;
using BlazorApp.Modules.InvoiceModule.CQRS.InvoiceGet.Models;

namespace BlazorApp.Modules.InvoiceModule.CQRS.InvoiceGet;

public class InvoiceGetDefaultQuery(int invoiceId) : InvoiceRequest<Result<InvoiceGetDataOut>>
{
  public int InvoiceId => invoiceId;
}