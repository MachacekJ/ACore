using ACore.Models.Result;
using ACore.Modules.MemoryCacheModule.Repositories;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheSaveHandler(IMemoryCacheModuleRepository cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleSaveCommand, Result>
{
  private readonly IMemoryCacheModuleRepository _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result> Handle(MemoryCacheModuleSaveCommand request, CancellationToken cancellationToken)
  {
    _cacheModule.Set(request.Key, request.Value);
    return Task.FromResult(Result.Success());
  }
}