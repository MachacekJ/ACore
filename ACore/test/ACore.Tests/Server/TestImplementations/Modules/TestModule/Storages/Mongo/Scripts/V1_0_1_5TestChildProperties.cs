﻿using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_1_5TestChildProperties : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.5");

  public override void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;

    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);
    //  db.CreateCollection(EfTestMongoStorageImpl.TestRootCategoryCollectionName);
    // logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", MongoTestStorageDbNames.ObjectNameMapping[nameof(TestAttributeAuditMongoEntity)].TableName, ext.DatabaseName);
  }
}