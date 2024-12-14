using MediatR;

namespace ACore.Server.Modules.LocalizationModule.CQRS;

public class LocalizationModuleRequest<TResponse> : IRequest<TResponse>;