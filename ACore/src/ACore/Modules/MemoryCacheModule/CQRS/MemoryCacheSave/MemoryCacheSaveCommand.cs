using ACore.Models.Cache;
using ACore.Models.Result;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheModuleSaveCommand(CacheKey key, object value) : MemoryCacheModuleRequest<Result>
{
    public CacheKey Key { get; } = key;
    public object Value { get; } = value;
}