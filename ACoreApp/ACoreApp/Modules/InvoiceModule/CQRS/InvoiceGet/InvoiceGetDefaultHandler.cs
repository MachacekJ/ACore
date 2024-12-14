using ACore.Results;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACoreApp.Modules.InvoiceModule.Configuration;
using ACoreApp.Modules.InvoiceModule.CQRS.InvoiceGet.Models;
using Microsoft.Extensions.Options;

namespace ACoreApp.Modules.InvoiceModule.CQRS.InvoiceGet;

public class InvoiceGetDefaultHandler(IRepositoryResolver repositoryResolver, IOptions<InvoiceModuleOptions> opt) : InvoiceRequestHandler<InvoiceGetDefaultQuery, Result<InvoiceGetDataOut>>(repositoryResolver, opt.Value)
{
  public override async Task<Result<InvoiceGetDataOut>> Handle(InvoiceGetDefaultQuery request, CancellationToken cancellationToken)
  {
    await Task.Delay(0, cancellationToken);
    return Result.Success(new InvoiceGetDataOut());
  }
}