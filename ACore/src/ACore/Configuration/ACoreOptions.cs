using ACore.Services.ACoreCache.Configuration;

namespace ACore.Configuration;

public class ACoreOptions
{
  public string SaltForHash { get; init; } = string.Empty;
  public ACoreCacheOptions ACoreCacheOptions { get; init; } = new();
}