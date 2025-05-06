using ACore.Results;
using ACore.Server.Modules.SettingsDbModule.CQRS;
using BlazorApp.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlazorApp.Modules.CustomerModule.CQRS;

public class CustomerPipelineBehavior<TRequest, TResponse>(IOptions<BlazorAppOptions> serverOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : SettingsDbModuleRequest<TResponse>
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