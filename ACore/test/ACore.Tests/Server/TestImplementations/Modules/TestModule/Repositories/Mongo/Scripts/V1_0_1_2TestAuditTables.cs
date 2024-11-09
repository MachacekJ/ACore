using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Scripts;

public class V1_0_1_2TestAuditTables : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.2");

  public override void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;

    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);

    var collectionName = DefaultNames.ObjectNameMapping[nameof(TestAuditEntity)].TableName;
    db.CreateCollection(collectionName);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, ext.DatabaseName);

    var collectionName2 = DefaultNames.ObjectNameMapping[nameof(TestValueTypeEntity)].TableName;
    db.CreateCollection(collectionName2);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName2, ext.DatabaseName);
    
    var collectionName3 = DefaultNames.ObjectNameMapping[nameof(TestNoAuditEntity)].TableName;
    db.CreateCollection(collectionName3);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, ext.DatabaseName);
  }
}