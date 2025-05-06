using ACore.Server.Repository.Contexts.EF.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_1_1Fake1NoAuditTable : EFVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l = new()
            {
                @"
CREATE TABLE fake1_no_audit
(
    fake1_no_audit_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    created timestamp
);
"
            };


            return l;
        }
    }
}