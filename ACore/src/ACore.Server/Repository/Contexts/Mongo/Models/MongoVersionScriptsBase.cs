using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Server.Repository.Contexts.Mongo.Models;

public abstract class MongoVersionScriptsBase
{
  /// <summary>
  /// Version of update.
  /// </summary>
  public abstract Version Version { get; }

  public abstract Task AfterScriptRunCode(IMongoDatabase mongoDatabase, ILogger<MongoContextBase> logger);

}