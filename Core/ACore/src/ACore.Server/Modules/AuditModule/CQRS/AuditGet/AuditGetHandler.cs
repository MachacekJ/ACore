using ACore.Results;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetHandler<TPK>(IRepositoryResolver repositoryResolver, IOptions<AuditModuleOptions> auditModuleOptions) : AuditModuleRequestHandler<AuditGetQuery<TPK>, Result<AuditGetDataOut<TPK>[]>>(repositoryResolver, auditModuleOptions.Value)
{
  public override async Task<Result<AuditGetDataOut<TPK>[]>> Handle(AuditGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null)
      throw new Exception($"Primary key is not found. TableName: {request.TableName}; Schema: {request.SchemaName ?? string.Empty}");

    var r = (await ReadFromRepository().AuditItemsAsync(request.TableName, request.PKValue, request.SchemaName))
      .Select(AuditGetDataOut<TPK>.Create).ToArray();

    return Result.Success(r);
  }
}