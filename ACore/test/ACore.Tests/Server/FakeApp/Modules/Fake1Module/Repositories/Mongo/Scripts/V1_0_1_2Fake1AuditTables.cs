using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Scripts;

internal class V1_0_1_2Fake1AuditTables : MongoVersionScriptsBase
{
  public override Version Version => new("1.0.0.2");

  public override async Task AfterScriptRunCode(IMongoDatabase mongoDatabase, ILogger<MongoContextBase> logger)
  {
    var db = mongoDatabase;

    var collectionName = typeof(Fake1AuditEntity).GetCollectionName();
    await db.CreateCollectionAsync(collectionName);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, db.DatabaseNamespace);

    var collectionName2 = typeof(Fake1NoAuditEntity).GetCollectionName();
    await db.CreateCollectionAsync(collectionName2);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName2, db.DatabaseNamespace);

    var collectionName3 = typeof(Fake1ValueTypeEntity).GetCollectionName();
    await db.CreateCollectionAsync(collectionName3);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, db.DatabaseNamespace);
  }
}