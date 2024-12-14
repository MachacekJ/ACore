using ACore.Models.Result;
using ACore.Modules.MemoryCacheModule.Services;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveKeyHandler(IMemoryCacheModuleRepository cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveKeyCommand, Result<bool>>
{
  private readonly IMemoryCacheModuleRepository _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveKeyCommand request, CancellationToken cancellationToken)
  {
    if (request.Key != null)
      _cacheModule.Remove(request.Key);
    
    return Task.FromResult(Result.Success(true));
  }
}