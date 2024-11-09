using ACore.Server.Storages.Contexts.EF.Scripts;

namespace ACore.Server.Modules.AuditModule.Repositories.SQL.PG.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>();
            all.Add(new V1_0_0_1AuditTables());
            return all;
        }
    }
}