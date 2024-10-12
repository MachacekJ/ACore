using ACore.Base.CQRS.Results;
using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    return await PerformWriteActionWithData<IAuditStorageModule>((storage) 
      => new SaveProcessExecutor(request.SaveInfoItem, storage, storage.SaveAuditAsync(request.SaveInfoItem)));
  }
}