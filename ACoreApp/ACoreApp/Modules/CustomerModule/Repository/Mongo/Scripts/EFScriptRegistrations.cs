using ACore.Server.Repository.Contexts.Mongo.Models;

namespace ACoreApp.Modules.CustomerModule.Repository.Mongo.Scripts;

internal static class EFScriptRegistrations
{
    public static IEnumerable<MongoVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<MongoVersionScriptsBase>
            {
                new V1_0_0_1CustomerCollection()
            };
            return all;
        }
    }
}