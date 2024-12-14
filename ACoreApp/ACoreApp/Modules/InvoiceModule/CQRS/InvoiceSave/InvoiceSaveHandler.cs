using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACoreApp.Modules.InvoiceModule.Configuration;
using Microsoft.Extensions.Options;

namespace ACoreApp.Modules.InvoiceModule.CQRS.InvoiceSave;

public class InvoiceSaveHandler(IRepositoryResolver repositoryResolver, IOptions<InvoiceModuleOptions> opt) : InvoiceRequestHandler<InvoiceSaveCommand, Result>(repositoryResolver, opt.Value)
{
  public override async Task<Result> Handle(InvoiceSaveCommand request, CancellationToken cancellationToken)
  {
    return await DeleteFromRepositories(s => new RepositoryExecutorItem(s.SaveInvoice(request.Invoice, cancellationToken)));
  }
}