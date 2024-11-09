using ACore.CQRS.Pipelines.Helpers;
using ACore.Models.Result;
using MediatR;
using Microsoft.Extensions.Options;
using SampleServerPackage.Configuration;

namespace SampleServerPackage.ToDoModulePG.CQRS;

internal class ToDoModulePipelineBehavior<TRequest, TResponse>(IOptions<SampleServerOptions> samplePackageOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : ToDoModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(samplePackageOptions.Value.ToDoModuleOptions, nameof(SampleServerServiceExtensions.AddSampleServerModule), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");
    
    return await next();
  }
}