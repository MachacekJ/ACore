using ACore.CQRS.Pipelines.Helpers;
using ACore.Results;
using ACore.Tests.Server.FakeApp.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS;

internal class Fake1PipelineBehavior<TRequest, TResponse>(IOptions<FakeAppOptions> testOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : Fake1Request<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(testOptions.Value.TestModuleOptions, nameof(FakeAppServiceExtensions.RegisterFakeAppContainer), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");
    
    return await next();
  }
}