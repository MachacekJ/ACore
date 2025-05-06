namespace ACore.Services.Localization.Models
{
  /// <summary>
  /// Scope for retrieve localization data from db.
  /// </summary>
  [Flags]
  public enum ACoreLocalizationScopeEnum
  {
    Server = 1 << 0,
    Client = 1 << 1
  }
}