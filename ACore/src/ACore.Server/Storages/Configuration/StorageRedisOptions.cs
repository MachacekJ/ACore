using StackExchange.Redis;

namespace ACore.Server.Storages.Configuration;

public class StorageRedisOptions(string connectionString, string instanceName)
{
  public string ConnectionString { get; set; } = connectionString;
  public string InstanceName { get; set; } =  instanceName;
  public string? Password { get; set; }
  public string? UserName { get; set; }
  
  public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(5);

  public ConfigurationOptions GetConfigurationOptions()
  {
   return  new ConfigurationOptions()
    {
      Password = Password,
      User = UserName,
      ClientName = InstanceName,
      EndPoints = { { ConnectionString } }
    };
  }

}