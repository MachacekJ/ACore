using ACore.Results;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;

public class SettingsDbGetQuery(string key, bool isRequired = false) : SettingsDbModuleRequest<Result<string?>>
{
  public string Key { get; } = key;
  public bool IsRequired { get; } = isRequired;
}