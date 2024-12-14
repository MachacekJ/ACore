using ACore.Results;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveHandler(IRepositoryResolver repositoryResolver, IOptions<AuditModuleOptions> auditModuleOptions) 
  : AuditModuleRequestHandler<AuditSaveCommand, Result>(repositoryResolver, auditModuleOptions.Value)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    return await DeleteFromRepositories((repository) 
      => new RepositoryExecutorItem(repository.SaveAuditAsync(request.EntityEventItem)));
  }
}