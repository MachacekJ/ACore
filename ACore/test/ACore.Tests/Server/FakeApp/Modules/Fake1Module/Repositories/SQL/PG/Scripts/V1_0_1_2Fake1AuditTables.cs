using ACore.Server.Repository.Contexts.EF.Models;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG.Scripts;

internal class V1_0_1_2Fake1AuditTables : EFVersionScriptsBase
{
    public override Version Version => new("1.0.0.2");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l =
            [
                @"
CREATE TABLE fake1_audit
(
    fake1_audit_id INT GENERATED ALWAYS AS IDENTITY
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