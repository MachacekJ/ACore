using ACore.Base.Cache;

namespace ACore.Server.Storages;

public static class CacheMainCategories
{
  public static CacheCategory Entity => new(nameof(Entity));
}