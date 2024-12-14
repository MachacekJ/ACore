using ACore.Server.Repository.Contexts.EF.Models;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.EF.PG.Scripts;

internal static class EFScriptRegistrations 
{
    public static List<EFVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<EFVersionScriptsBase> { new V1_0_0_1SettingsTable() };
            return all;
        }
    }
}