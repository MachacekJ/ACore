using ACore.Server.Repository.Contexts.Mongo.Models;

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo.Scripts;

internal static class MongoScriptRegistrations 
{
    public static IEnumerable<MongoVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<MongoVersionScriptsBase>
            {
                new V1_0_0_1AuditCollection()
            };
            return all;
        }
    }
}