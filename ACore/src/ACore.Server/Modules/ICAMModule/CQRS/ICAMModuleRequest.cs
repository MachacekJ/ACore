using ACore.Models.Result;
using MediatR;

namespace ACore.Server.Modules.ICAMModule.CQRS;

public class ICAMModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;