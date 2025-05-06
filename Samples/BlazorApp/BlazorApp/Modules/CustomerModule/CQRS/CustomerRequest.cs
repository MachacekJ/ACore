using MediatR;

namespace BlazorApp.Modules.CustomerModule.CQRS;

public class CustomerRequest<TResponse> : IRequest<TResponse>;
