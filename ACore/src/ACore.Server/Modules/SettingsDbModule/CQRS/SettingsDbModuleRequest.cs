using ACore.Models.Result;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public class SettingsDbModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;