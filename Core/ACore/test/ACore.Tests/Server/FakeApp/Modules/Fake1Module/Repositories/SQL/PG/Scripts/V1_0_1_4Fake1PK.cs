using ACore.Server.Repository.Contexts.EF.Models;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG.Scripts;

public class V1_0_1_4Fake1PK : EFVersionScriptsBase
{
    public override Version Version => new("1.0.0.4");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l = new()
            {
                @"
CREATE TABLE fake1_pk_guid
(
    fake1_pk_guid_id UUID
        PRIMARY KEY,
    name VARCHAR(20)
);

CREATE TABLE fake1_pk_string
(
    fake1_pk_string_id VARCHAR(50)
        PRIMARY KEY,
    name VARCHAR(20)
);


CREATE TABLE fake1_pk_long
(
    fake1_pk_long_id BIGINT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(20)
);
"
            };


            return l;
        }
    }
}