namespace ACore.Modules.LocalizationModule.Models;

public class LanguageItem(int lcid, string name)
{
  public string Name => name;
  public int LCID => lcid;
}