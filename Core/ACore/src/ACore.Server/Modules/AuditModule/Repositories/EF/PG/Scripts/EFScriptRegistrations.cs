using ACore.Server.Repository.Contexts.EF.Models;

namespace ACore.Server.Modules.AuditModule.Repositories.EF.PG.Scripts;

internal static class EFScriptRegistrations 
{
    public static List<EFVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<EFVersionScriptsBase>();
            all.Add(new V1_0_0_1AuditTables());
            return all;
        }
    }
}