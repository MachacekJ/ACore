using MediatR;

namespace ACoreApp.Modules.InvoiceModule.CQRS;

public class InvoiceRequest<TResponse> : IRequest<TResponse>;
