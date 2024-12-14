using ACore.Client.Modules.LocalizationModule.Configuration;

namespace ACore.Client.Configuration;

public class ACoreClientOptions
{
  public LocalizationClientModuleOptions LocalizationClientModuleOptions { get; set; } = new();
}