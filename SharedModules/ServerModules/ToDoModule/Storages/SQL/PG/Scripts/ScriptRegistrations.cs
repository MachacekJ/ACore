using ACore.Server.Storages.Contexts.EF.Scripts;

namespace ServerModules.ToDoModule.Storages.SQL.PG.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>
            {
                new V1_0_1_1ToDoStructure()
            };
            return all;
        }
    }
}