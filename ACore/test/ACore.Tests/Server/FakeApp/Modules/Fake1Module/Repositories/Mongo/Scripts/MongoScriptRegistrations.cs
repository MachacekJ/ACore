using ACore.Server.Repository.Contexts.Mongo.Models;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Scripts;

internal static class MongoScriptRegistrations 
{
  public static IEnumerable<MongoVersionScriptsBase> AllVersions
  {
    get
    {
      var all = new List<MongoVersionScriptsBase>
      {
        new V1_0_1_2Fake1AuditTables()
      };
      return all;
    }
  }
}