using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using BlazorApp.Modules.InvoiceModule.Models;
using BlazorApp.Modules.InvoiceModule.Repository.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Modules.InvoiceModule.Repository.EF;

internal abstract class InvoiceSqlRepositoryImpl : EFContextBase, IInvoiceRepository
{
  public DbSet<InvoiceEntity> Invoices { get; set; }

  protected InvoiceSqlRepositoryImpl(DbContextOptions options, IACoreServerCurrentScope serverCurrentScope, ILogger<InvoiceSqlRepositoryImpl> logger) : base(options, serverCurrentScope, logger)
  {
    RegisterDbSet(Invoices);
  }


  protected override string ModuleName => nameof(IInvoiceRepository);

  public async Task<InvoiceItem?> GetInvoiceItemByInternalId(int invoiceId, CancellationToken cancellationToken)
  {
    var invoice = await Invoices.Include(e=>e.InvoiceItems).FirstOrDefaultAsync(e => e.Id == invoiceId, cancellationToken);
    return invoice?.ToItem();
  }

  public async Task<InvoiceItem[]> AllInvoices(CancellationToken cancellationToken)
  {
    return await Invoices.Select(e => e.ToItem()).ToArrayAsync(cancellationToken);
  }

  public Task<InvoiceItem> DefaultInvoice(CancellationToken cancellationToken)
  {
    return Task.FromResult(new InvoiceItem());
  }

  public async Task<RepositoryOperationResult> SaveInvoice(InvoiceItem invoice, CancellationToken cancellationToken)
    => await Save<InvoiceEntity, int>(invoice.ToEntity());
}