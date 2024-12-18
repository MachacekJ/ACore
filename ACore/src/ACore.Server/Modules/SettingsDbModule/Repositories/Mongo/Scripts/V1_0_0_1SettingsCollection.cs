﻿using ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Models;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Server.Modules.SettingsDbModule.Repositories.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1SettingsCollection : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");

  public override void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;

    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);

    var collectionName = CollectionNames.ObjectNameMapping[nameof(SettingsPKMongoEntity)].TableName;
    db.CreateCollection(collectionName);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, ext.DatabaseName);
  }
}