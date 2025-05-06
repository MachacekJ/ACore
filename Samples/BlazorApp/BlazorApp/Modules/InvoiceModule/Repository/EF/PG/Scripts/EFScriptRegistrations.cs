using ACore.Server.Repository.Contexts.EF.Models;

namespace BlazorApp.Modules.InvoiceModule.Repository.EF.PG.Scripts;

internal static class EFScriptRegistrations
{
  public static List<EFVersionScriptsBase> AllVersions
  {
    get
    {
      var all = new List<EFVersionScriptsBase>
      {
        new V1_0_0_1BasicStructure(),
        new V1_0_0_2SeedStatus()
      };
      return all;
    }
  }
}