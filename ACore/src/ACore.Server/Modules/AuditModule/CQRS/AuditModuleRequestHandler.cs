using ACore.Results;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.Repositories;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS;

public abstract class AuditModuleRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, AuditModuleOptions auditModuleOptions)
  : RepositoryRequestHandler<IAuditRepository, TRequest, TResponse>(repositoryResolver, auditModuleOptions)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
 
}