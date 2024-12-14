using ACore.Server.Repository.Contexts.EF.Models;

namespace ACore.Server.Modules.LocalizationModule.Repositories.EF.PG.Scripts
{
  public static class EFScriptRegistrations
  {
    public static List<EFVersionScriptsBase> AllVersions
    {
      get
      {
        var all = new List<EFVersionScriptsBase> { new V1_0_0_01LocalizationTable() };
        return all;
      }
    }
  }
}