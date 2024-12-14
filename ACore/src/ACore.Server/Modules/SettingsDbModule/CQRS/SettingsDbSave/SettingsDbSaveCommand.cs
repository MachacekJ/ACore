using ACore.Repository.Definitions.Models;
using ACore.Results;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveCommand(RepositoryTypeEnum repositoryType, string key, string value, bool isSystem = false) : SettingsDbModuleRequest<Result>
{
  public RepositoryTypeEnum RepositoryType { get; } = repositoryType;
  public string Key { get; } = key;
  public string Value { get; } = value;
  public bool IsSystem { get; } = isSystem;

  public SettingsDbSaveCommand(string key, string value, bool isSystem = false) : this(RepositoryTypeEnum.AllDb, key, value, isSystem)
  {
  }
}