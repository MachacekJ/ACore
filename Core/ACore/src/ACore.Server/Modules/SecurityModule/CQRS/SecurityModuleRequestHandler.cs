using ACore.Results;
using MediatR;

namespace ACore.Server.Modules.SecurityModule.CQRS;

public abstract class SecurityModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}