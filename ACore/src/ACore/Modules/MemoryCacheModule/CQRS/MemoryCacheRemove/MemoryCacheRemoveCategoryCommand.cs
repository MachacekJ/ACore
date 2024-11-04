using ACore.Models.Cache;
using ACore.Models.Result;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveCategoryCommand(CacheCategory mainCategory, CacheCategory? subCategory = null) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheCategory? MainCategory { get; } = mainCategory;
  public CacheCategory? SubCategory { get; } = subCategory;
}