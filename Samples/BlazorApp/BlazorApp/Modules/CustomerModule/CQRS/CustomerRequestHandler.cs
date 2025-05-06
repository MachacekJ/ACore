using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using BlazorApp.Modules.CustomerModule.Configuration;
using BlazorApp.Modules.CustomerModule.Repository;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlazorApp.Modules.CustomerModule.CQRS;

public abstract class CustomerRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, IOptions<CustomerModuleOptions> opt) : RepositoryRequestHandler<ICustomerRepository, TRequest, TResponse>(repositoryResolver, opt.Value)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
 
}