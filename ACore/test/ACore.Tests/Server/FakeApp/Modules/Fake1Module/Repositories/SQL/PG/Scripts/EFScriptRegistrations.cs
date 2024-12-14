using ACore.Server.Repository.Contexts.EF.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG.Scripts;

internal static class EFScriptRegistrations
{
    public static List<EFVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<EFVersionScriptsBase>
            {
                new V1_0_1_1Fake1NoAuditTable(),
                new V1_0_1_2Fake1AuditTables(),
                new V1_0_1_3Fake1AuditTypes(),
                new V1_0_1_4Fake1PK(),
                //new V1_0_1_5TestParentChild()
            };
            return all;
        }
    }
}