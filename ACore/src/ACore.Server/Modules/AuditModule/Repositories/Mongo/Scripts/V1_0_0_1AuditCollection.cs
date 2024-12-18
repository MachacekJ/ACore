﻿using ACore.Server.Modules.AuditModule.Repositories.Mongo.Models;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1AuditCollection : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");

  public override void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;

    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);
    db.CreateCollection(DefaultNames.AuditCollectionName);
    var col = db.GetCollection<AuditMongoEntity>(DefaultNames.AuditCollectionName);

    var index = Builders<AuditMongoEntity>.IndexKeys.Ascending(e => e.ObjectId);
    col.Indexes.CreateOneAsync(new CreateIndexModel<AuditMongoEntity>(index));

    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", DefaultNames.AuditCollectionName, ext.DatabaseName);
  }
}