using ACore.Results;
using BlazorApp.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlazorApp.Modules.InvoiceModule.CQRS;

public class InvoicePipelineBehavior<TRequest, TResponse>(IOptions<BlazorAppOptions> serverOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : InvoiceRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    //var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();

    // if (!moduleBehaviorHelper.CheckIfModuleIsActive(serverOptions.Value.InvoiceModuleOptions, nameof(ACoreServerServiceExtensions.AddACoreServer), out var resultError))
    //   return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    return await next();
  }
  
}