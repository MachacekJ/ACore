using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF.Models;

namespace ACore.Server.Modules.SettingsDbModule.Repositories;

public interface ISettingsDbModuleRepository : IRepository
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task<RepositoryOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false);
}