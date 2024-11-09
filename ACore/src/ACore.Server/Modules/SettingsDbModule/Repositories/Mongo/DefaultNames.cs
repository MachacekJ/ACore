using System.Linq.Expressions;
using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;
using ACore.Server.Storages.Definitions.EF;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.Mongo;
#pragma warning disable CS8603 // Possible null reference return.

public static class CollectionNames
{
  public static Dictionary<string, EFDbNames> ObjectNameMapping => new()
  {
    { nameof(SettingsPKMongoEntity), new EFDbNames("setting", SettingMongoEntityColumnNames) },
  };
  
  private static Dictionary<Expression<Func<SettingsPKMongoEntity, object>>, string> SettingMongoEntityColumnNames => new()
  {
    { e => e.Id, "_id" },
    { e => e.Key, "key" },
    { e => e.Value, "value" },
    { e => e.IsSystem, "isSystem" }
  };
}