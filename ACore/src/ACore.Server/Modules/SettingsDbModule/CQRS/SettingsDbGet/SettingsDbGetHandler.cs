﻿using ACore.Models.Result;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;

public class SettingsDbGetHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsDbGetQuery, Result<string?>>(storageResolver)
{
  private readonly IStorageResolver _storageResolver = storageResolver;

  public override async Task<Result<string?>> Handle(SettingsDbGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = _storageResolver.ReadFromStorage<ISettingsDbModuleRepository>(request.StorageType);
    var res= await storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
    return Result.Success(res);
  }
}