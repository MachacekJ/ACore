using ACore.Models.Result;
using ACore.Modules.MemoryCacheModule.Services;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveCategoryHandler(IMemoryCacheModuleRepository cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveCategoryCommand, Result<bool>>
{
  private readonly IMemoryCacheModuleRepository _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveCategoryCommand request, CancellationToken cancellationToken)
  {
    if (request.MainCategory != null)
      _cacheModule.RemoveCategory(request.MainCategory, request.SubCategory);

    return Task.FromResult(Result.Success(true));
  }
}