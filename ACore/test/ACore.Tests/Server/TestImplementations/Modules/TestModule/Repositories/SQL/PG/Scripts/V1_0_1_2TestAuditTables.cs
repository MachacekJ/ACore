﻿using ACore.Server.Storages.Contexts.EF.Scripts;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.PG.Scripts;

internal class V1_0_1_2TestAuditTables : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.2");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l =
            [
                @"
CREATE TABLE test_audit
(
    test_audit_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    null_value VARCHAR(50),
    null_value2 VARCHAR(50),
    null_value3 VARCHAR(50),
    not_auditable_column VARCHAR(50),
    created timestamp
);
"
            ];
            return l;
        }
    }
}