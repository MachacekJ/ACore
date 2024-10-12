using ACore.Server.Storages.Contexts.EF.Scripts;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions
  {
    get
    {
      var all = new List<DbVersionScriptsBase>
      {
       // new V1_0_1_1TestTable(),
        new V1_0_1_2TestAuditTables(),
        //  new V1_0_1_3TestAuditTypes(),
        //   new V1_0_1_4TestPK()
        new V1_0_1_5TestChildProperties()
      };
      return all;
    }
  }
}