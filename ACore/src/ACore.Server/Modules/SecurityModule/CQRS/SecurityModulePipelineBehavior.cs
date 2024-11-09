using ACore.CQRS.Pipelines.Helpers;
using ACore.Models.Result;
using ACore.Server.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SecurityModule.CQRS;

public class SecurityModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreServerOptions> serverOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : SecurityModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();

    if (!moduleBehaviorHelper.CheckIfModuleIsActive(serverOptions.Value.SettingsDbModuleOptions, nameof(ACoreServerServiceExtensions.AddACoreServer), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    return await next();
  }
}