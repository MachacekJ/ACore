using MediatR;

namespace ACore.Modules.LocalizationModule.CQRS;

public class LocalizationModuleRequest<TResponse> : IRequest<TResponse>;
