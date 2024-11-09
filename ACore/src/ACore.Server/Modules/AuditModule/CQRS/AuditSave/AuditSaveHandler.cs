using ACore.Models.Result;
using ACore.Server.Modules.AuditModule.Repositories;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveHandler(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageParallelAction<IAuditRepository>((storage) 
      => new StorageExecutorItem(storage.SaveAuditAsync(request.EntityEventItem)));
  }
}