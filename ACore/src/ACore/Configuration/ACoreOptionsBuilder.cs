using ACore.Services.ACoreCache.Configuration;

namespace ACore.Configuration;

public class ACoreOptionsBuilder
{
  private string _saltForHash = string.Empty;
  private readonly ACoreCacheOptions _cacheOptions = new();
  
  private ACoreOptionsBuilder()
  {
  }

  public static ACoreOptionsBuilder Empty() => new();

  public ACoreOptionsBuilder AddSaltForHash(string salt)
  {
    _saltForHash = salt;
    return this;
  }

  public ACoreOptionsBuilder AddACoreCache(Action<ACoreCacheOptions> optionsAction)
  {
    optionsAction(_cacheOptions);
    return this;
  }

  public ACoreOptions Build()
  {
    return new ACoreOptions
    {
      SaltForHash = _saltForHash,
      ACoreCacheOptions = _cacheOptions
    };
  }
}