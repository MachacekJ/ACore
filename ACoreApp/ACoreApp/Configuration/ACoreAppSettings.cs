// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ACoreApp.Configuration;

public class ACoreAppSettings
{
  public string PG { get; set; }
  public MongoSettings Mongo { get; set; }
  public RedisSettings Redis { get; set; }
}

public class RedisSettings
{
  public string Server { get; set; }
  public string Password { get; set; }
  public string InstanceName { get; set; }
}

public class MongoSettings
{
  public string ConnectionString { get; set; }
  public string CollectionName { get; set; }
}