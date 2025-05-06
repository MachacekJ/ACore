using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using BlazorApp.Modules.InvoiceModule.Configuration;
using BlazorApp.Modules.InvoiceModule.Repository;
using MediatR;

namespace BlazorApp.Modules.InvoiceModule.CQRS;

public abstract class InvoiceRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, InvoiceModuleOptions opt) : RepositoryRequestHandler<IInvoiceRepository, TRequest, TResponse>(repositoryResolver, opt)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
}