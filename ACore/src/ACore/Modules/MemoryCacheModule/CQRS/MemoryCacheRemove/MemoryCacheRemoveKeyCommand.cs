using ACore.Models.Cache;
using ACore.Models.Result;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveKeyCommand(CacheKey key) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheKey? Key { get; } = key;
}