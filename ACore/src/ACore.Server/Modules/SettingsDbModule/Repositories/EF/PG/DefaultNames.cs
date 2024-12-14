using System.Linq.Expressions;
using ACore.Server.Modules.SettingsDbModule.Repositories.EF.Models;
using ACore.Server.Repository.Contexts.EF.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Server.Modules.SettingsDbModule.Repositories.EF.PG;

internal static class DefaultNames
{
  public static Dictionary<string, EFInternalName> ObjectNameMapping => new()
  {
    { nameof(SettingsEntity), new EFInternalName("setting", SettingsEntityColumnNames) },
  };

  private static Dictionary<Expression<Func<SettingsEntity, object>>, string> SettingsEntityColumnNames => new()
  {
    { e => e.Id, "setting_id" },
    { e => e.Key, "key" },
    { e => e.Value, "value" },
    { e => e.IsSystem, "is_system" }
  };

}