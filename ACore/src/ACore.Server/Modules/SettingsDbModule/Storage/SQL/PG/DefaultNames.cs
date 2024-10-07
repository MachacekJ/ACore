﻿using System.Linq.Expressions;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Definitions.EF;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG;

public static class DefaultNames
{
  public static Dictionary<string, EFNameDefinition> ObjectNameMapping => new()
  {
    { nameof(SettingsEntity), new EFNameDefinition("setting", SettingsEntityColumnNames) },
  };

  private static Dictionary<Expression<Func<SettingsEntity, object>>, string> SettingsEntityColumnNames => new()
  {
    { e => e.Id, "setting_id" },
    { e => e.Key, "key" },
    { e => e.Value, "value" },
    { e => e.IsSystem, "is_system" }
  };

}