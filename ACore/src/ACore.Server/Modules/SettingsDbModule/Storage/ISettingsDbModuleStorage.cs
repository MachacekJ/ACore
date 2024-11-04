using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF.Models;

namespace ACore.Server.Modules.SettingsDbModule.Storage;

public interface ISettingsDbModuleStorage : IStorage
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task<DatabaseOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false);
}