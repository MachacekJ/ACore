using ACore.Server.Repository;
using ACore.Server.Repository.Results;
using ACoreApp.Modules.InvoiceModule.Models;

namespace ACoreApp.Modules.InvoiceModule.Repository;

public interface IInvoiceRepository : IDbRepository
{
  Task<InvoiceItem?> GetInvoiceItemByInternalId(int invoiceId, CancellationToken cancellationToken);
  Task<InvoiceItem[]> AllInvoices(CancellationToken cancellationToken);
  
  /// <summary>
  /// Get default invoice value for new item for form.
  /// </summary>
  Task<InvoiceItem> DefaultInvoice(CancellationToken cancellationToken);
  
  Task<RepositoryOperationResult> SaveInvoice(InvoiceItem invoice, CancellationToken cancellationToken);
}