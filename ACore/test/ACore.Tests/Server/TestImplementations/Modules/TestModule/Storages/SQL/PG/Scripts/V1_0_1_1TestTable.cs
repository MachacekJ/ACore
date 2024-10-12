using ACore.Server.Storages.Contexts.EF.Scripts;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_1_1TestTable : DbVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l = new()
            {
                @"
CREATE TABLE test_no_audit
(
    test_id INT GENERATED ALWAYS AS IDENTITY
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