using ACore.CQRS.Pipelines.Helpers;
using ACore.Models.Result;
using ACore.Tests.Server.TestImplementations.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS;

internal class TestModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreTestOptions> testOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : TestModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(testOptions.Value.TestModuleOptions, nameof(ACoreTestServiceExtensions.AddACoreTest), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");
    
    return await next();
  }
}