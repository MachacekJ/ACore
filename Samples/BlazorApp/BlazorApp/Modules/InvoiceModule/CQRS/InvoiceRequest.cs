using MediatR;

namespace BlazorApp.Modules.InvoiceModule.CQRS;

public class InvoiceRequest<TResponse> : IRequest<TResponse>;
