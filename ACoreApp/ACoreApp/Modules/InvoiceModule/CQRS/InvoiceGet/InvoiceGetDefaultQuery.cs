using ACore.Results;
using ACoreApp.Modules.InvoiceModule.CQRS.InvoiceGet.Models;

namespace ACoreApp.Modules.InvoiceModule.CQRS.InvoiceGet;

public class InvoiceGetDefaultQuery(int invoiceId) : InvoiceRequest<Result<InvoiceGetDataOut>>
{
  public int InvoiceId => invoiceId;
}