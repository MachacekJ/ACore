using ACore.Models.Result;
using MediatR;

namespace ACore.Server.Modules.SecurityModule.CQRS;

public class SecurityModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;