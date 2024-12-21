using ACore.Services.ACoreCache.Configuration;

namespace ACore.Configuration;

public class ACoreOptions
{
  public string SaltForHash { get; set; } = string.Empty;
  public ACoreCacheOptions ACoreCacheOptions { get; set; } = new();
}