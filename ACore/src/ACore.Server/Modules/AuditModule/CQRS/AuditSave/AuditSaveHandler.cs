using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveHandler(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageParallelAction<IAuditStorageModule>((storage) 
      => new StorageExecutorItem(storage.SaveAuditAsync(request.EntityEventItem)));
  }
}