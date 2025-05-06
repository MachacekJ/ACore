using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using MongoDB.Driver;

namespace BlazorApp.Modules.CustomerModule.Repository.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1CustomerCollection : MongoVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");

  public override async Task AfterScriptRunCode(IMongoDatabase mongoDatabase, ILogger<MongoContextBase> logger)
  {
    var collectionName = typeof(SettingsPKMongoEntity).GetCollectionName();
    await mongoDatabase.CreateCollectionAsync(collectionName);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, mongoDatabase.DatabaseNamespace);
  }

  
}