using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(SettingsDbSaveCommand request, CancellationToken cancellationToken)
  {
    return await PerformWriteAction((storage)
      => new DeleteProcessExecutor(storage.Setting_SaveAsync(request.Key, request.Value, request.IsSystem)));
  }
}