using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACoreApp.Modules.CustomerModule.Configuration;
using ACoreApp.Modules.CustomerModule.Repository;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACoreApp.Modules.CustomerModule.CQRS;

public abstract class CustomerRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, IOptions<CustomerModuleOptions> opt) : RepositoryRequestHandler<ICustomerRepository, TRequest, TResponse>(repositoryResolver, opt.Value)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
 
}