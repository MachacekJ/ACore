using ACore.Services.ACoreCache.Configuration;

namespace ACore.Configuration;

public class ACoreOptionsBuilder
{
  private string _saltForHash = string.Empty;
  protected readonly ACoreCacheOptions CacheOptions = new();

  public static ACoreOptionsBuilder Empty() => new();

  public void AddSaltForHash(string salt)
    => _saltForHash = salt;
  
  public void AddACoreCache(Action<ACoreCacheOptions> optionsAction)
  => optionsAction(CacheOptions);
  
  public virtual ACoreOptions Build()
  {
    var res = new ACoreOptions();
    SetOptions(res);
    return res;
  }

  protected void SetOptions(ACoreOptions opt)
  {
    opt.SaltForHash = _saltForHash;
    opt.ACoreCacheOptions = CacheOptions;
  }
}