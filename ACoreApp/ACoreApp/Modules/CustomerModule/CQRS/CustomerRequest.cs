using MediatR;

namespace ACoreApp.Modules.CustomerModule.CQRS;

public class CustomerRequest<TResponse> : IRequest<TResponse>;
