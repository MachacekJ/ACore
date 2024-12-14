using ACore.Results;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories;
using MediatR;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS;

public abstract class Fake1RequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, Fake1ModuleOptions fake1ModuleOptions) : RepositoryRequestHandler<IFake1Repository, TRequest, TResponse>(repositoryResolver, fake1ModuleOptions)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
 
}