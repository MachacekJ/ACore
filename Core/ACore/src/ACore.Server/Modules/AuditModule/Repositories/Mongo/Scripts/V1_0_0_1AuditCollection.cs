using ACore.Server.Modules.AuditModule.Repositories.Mongo.Models;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1AuditCollection : MongoVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");

  public override async Task AfterScriptRunCode(IMongoDatabase mongoDatabase, ILogger<MongoContextBase> logger)
  {
    var auditCollectionName = typeof(AuditMongoEntity).GetCollectionName();
    await mongoDatabase.CreateCollectionAsync(auditCollectionName);
    var col = mongoDatabase.GetCollection<AuditMongoEntity>(auditCollectionName);

    var index = Builders<AuditMongoEntity>.IndexKeys.Ascending(e => e.ObjectId);
    await col.Indexes.CreateOneAsync(new CreateIndexModel<AuditMongoEntity>(index));

    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", auditCollectionName, mongoDatabase.DatabaseNamespace);
  }
}