using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using BlazorApp.Modules.InvoiceModule.Configuration;
using BlazorApp.Modules.InvoiceModule.CQRS.InvoiceGet.Models;
using Microsoft.Extensions.Options;

namespace BlazorApp.Modules.InvoiceModule.CQRS.InvoiceGet;

public class InvoiceGetDefaultHandler(IRepositoryResolver repositoryResolver, IOptions<InvoiceModuleOptions> opt) : InvoiceRequestHandler<InvoiceGetDefaultQuery, Result<InvoiceGetDataOut>>(repositoryResolver, opt.Value)
{
  public override async Task<Result<InvoiceGetDataOut>> Handle(InvoiceGetDefaultQuery request, CancellationToken cancellationToken)
  {
    await Task.Delay(0, cancellationToken);
    return Result.Success(new InvoiceGetDataOut());
  }
}