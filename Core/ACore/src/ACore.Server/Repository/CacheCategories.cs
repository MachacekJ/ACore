using ACore.Models.Cache;

namespace ACore.Server.Repository;

public static class CacheCategories
{
  public static CacheCategory Entity => new(nameof(Entity));
  public static CacheCategory Localization => new(nameof(Localization));
}