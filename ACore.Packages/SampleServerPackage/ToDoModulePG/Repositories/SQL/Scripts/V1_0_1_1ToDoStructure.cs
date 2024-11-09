using ACore.Server.Storages.Contexts.EF.Scripts;

namespace SampleServerPackage.ToDoModulePG.Repositories.SQL.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_1_1ToDoStructure : DbVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l = new()
            {
                @"
CREATE TABLE todo_list
(
    todo_list_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    created timestamp
);

CREATE TABLE todo_list_item
(
    todo_list_item_id BIGINT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    todo_list_id INT NOT NULL
        CONSTRAINT fk_todo_list_item__todo_list
            REFERENCES todo_list(todo_list_id),
    amount NUMERIC(19,4) NOT NULL,
    name VARCHAR(50),
    description TEXT,
    unit_price NUMERIC(19,4) NOT NULL,
    created timestamp
);
"
            };


            return l;
        }
    }
}