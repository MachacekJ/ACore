using ACore.Models.Cache;

namespace ACore.Configuration.Cache;

public class CacheOptions
{
  public List<CacheCategory> Categories { get; set; } = [];
}