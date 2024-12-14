using ACore.Server.Repository;
using ACore.Server.Repository.Results;

namespace ACore.Server.Modules.SettingsDbModule.Repositories;

public interface ISettingsDbModuleRepository : IDbRepository
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task<RepositoryOperationResult> Setting_SaveAsync(string key, string value, bool isSystem = false);
}