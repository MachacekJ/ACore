﻿using ACore.Server.Storages.Contexts.EF.Scripts;

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>
            {
                new V1_0_0_1AuditCollection()
            };
            return all;
        }
    }
}