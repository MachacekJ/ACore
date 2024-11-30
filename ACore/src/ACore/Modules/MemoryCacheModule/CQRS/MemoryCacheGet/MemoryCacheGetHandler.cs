using ACore.Models.Cache;
using ACore.Models.Result;
using ACore.Modules.MemoryCacheModule.Repositories;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;

public class MemoryMemoryCacheGetHandler(IMemoryCacheModuleRepository cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleGetQuery, Result<CacheValue?>>
{
    private readonly IMemoryCacheModuleRepository _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

    public override Task<Result<CacheValue?>> Handle(MemoryCacheModuleGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        CacheValue? cacheValue = null;
        var result = _cacheModule.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new CacheValue(value);

        return Task.FromResult(Result.Success(cacheValue));
    }
}