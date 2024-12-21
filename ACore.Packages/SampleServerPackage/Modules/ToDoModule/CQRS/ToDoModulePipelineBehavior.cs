using ACore.CQRS.Pipelines.Helpers;
using ACore.Models.Result;
using ACore.Server.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace SampleServerPackage.ToDoModulePG.CQRS;

internal class ToDoModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreServerOptions> aCoreServerOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : ToDoModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(aCoreServerOptions.Value.ToDoModuleOptions, nameof(ToDoModulePG), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");
    
    return await next();
  }
}