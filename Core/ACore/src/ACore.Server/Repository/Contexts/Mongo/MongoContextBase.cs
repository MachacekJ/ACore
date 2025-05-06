using ACore.Repository;
using ACore.Repository.Definitions.Models;
using ACore.Repository.Models;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Configuration.RepositoryTypes;
using ACore.Server.Repository.Contexts.Helpers;
using ACore.Server.Repository.Contexts.Helpers.Models;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ACore.Server.Repository.Contexts.Mongo;

public abstract class MongoContextBase : IRepository
{
  private string RepositoryVersionKey => $"RepositoryVersion_{nameof(RepositoryTypeEnum.Mongo)}_{ModuleName}";
  private bool _isDatabaseInFirstInit;
  private readonly IMongoDatabase _mongoDatabase;
  private readonly RepositoryMongoOptions _mongoOptions;
  private readonly IMediator _mediator;
  private readonly ILogger<MongoContextBase> _logger;
  private readonly DatabaseCRUDHelper _databaseCRUDHelper;

  protected abstract IEnumerable<MongoVersionScriptsBase> AllUpdateVersions { get; }
  protected abstract string ModuleName { get; }
  public RepositoryInfo RepositoryInfo  => new(ModuleName, RepositoryTypeEnum.Mongo);

  public RepositoryMongoOptions MongoOptions => _mongoOptions;
  protected IMongoDatabase MongoDatabase => _mongoDatabase;

  protected MongoContextBase(IACoreServerCurrentScope serverCurrentScope, RepositoryMongoOptions mongoOptions, IMediator mediator, ILogger<MongoContextBase> logger)
  {
    _mongoOptions = mongoOptions;
    _mediator = mediator;
    _logger = logger;
    var mongoClient = new MongoClient(_mongoOptions.ReadWriteConnectionString);
    _mongoDatabase = mongoClient.GetDatabase(_mongoOptions.CollectionName);
    _databaseCRUDHelper = new DatabaseCRUDHelper(serverCurrentScope, _logger);
  }

  protected async Task<RepositoryOperationResult> Save<TEntity>(IMongoCollection<TEntity> entityCollection, TEntity newData, string? oldValueHash = null)
    where TEntity : PKMongoEntity
  {
    var helperAuditInfo = AuditHelper<TEntity>();
    var crud = CRUDHelper(entityCollection);

    return await _databaseCRUDHelper.Save(newData, crud, helperAuditInfo,
      newData.Id == PKMongoEntity.EmptyId,
      _isDatabaseInFirstInit,
      oldValueHash
    );
  }

  private static DatabaseAuditDefinitions AuditHelper<TEntity>() where TEntity : PKMongoEntity
    => new(
      typeof(TEntity).GetCollectionName(),
      null, true,
      GetColumnAuditAttrInfo<TEntity>);

  private static DatabaseCRUDDefinitions<TEntity, ObjectId> CRUDHelper<TEntity>(IMongoCollection<TEntity> entityCollection) where TEntity : PKMongoEntity
    => new(
      () => PKMongoEntity.NewId,
      async (id) => await GetEntityById(entityCollection, id),
      async (ent) => await entityCollection.InsertOneAsync(ent),
      async ent => await entityCollection.ReplaceOneAsync(f => f.Id.Equals(ent.Id), ent),
      async id => await entityCollection.DeleteOneAsync(f => f.Id.Equals(id)),
      () => Task.CompletedTask);


  protected async Task<RepositoryOperationResult> Delete<TEntity>(IMongoCollection<TEntity> entityCollection, ObjectId id, string? oldValueHash = null)
    where TEntity : PKMongoEntity
  {
    var helperAuditInfo = AuditHelper<TEntity>();
    var crud = CRUDHelper(entityCollection);

    return await _databaseCRUDHelper.Delete(id, crud, helperAuditInfo, oldValueHash);
  }

  private static async Task<TEntity?> GetEntityById<TEntity>(IMongoCollection<TEntity> entityCollection, ObjectId id)
    where TEntity : PKMongoEntity
  {
    var filter = Builders<TEntity>.Filter.Eq(u => u.Id, id);
    using var cursor = await entityCollection.FindAsync(filter);
    return await cursor.FirstOrDefaultAsync();
  }

  public async Task UpSchema()
  {
    var allVersions = AllUpdateVersions.ToList();
    _isDatabaseInFirstInit = await DatabaseHasFirstUpdate();
    var lastVersion = await _databaseCRUDHelper.GetVersion(RepositoryVersionKey, _isDatabaseInFirstInit);
    var updatedToVersion = lastVersion;

    if (allVersions.Count != 0)
    {
      if (_mongoOptions.EnableTransactions)
      {
        using var session = await _mongoDatabase.Client.StartSessionAsync();
        session.StartTransaction();
        try
        {
          updatedToVersion = await UpdateSchema(allVersions, lastVersion, _mongoDatabase);
          await session.CommitTransactionAsync();
        }
        catch (Exception e)
        {
          _logger.LogCritical(e, "Mongo update failed.");
          await session.AbortTransactionAsync();
          throw;
        }
      }
      else
      {
        await UpdateSchema(allVersions, lastVersion, _mongoDatabase);
      }
    }

    if (updatedToVersion <= lastVersion)
      return;

    if (this is ISettingsDbModuleRepository settingsDbModuleRepository)
    {
      await settingsDbModuleRepository.Setting_SaveAsync(RepositoryVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await _mediator.Send(new SettingsDbSaveCommand(RepositoryTypeEnum.Mongo, RepositoryVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateSchema(List<MongoVersionScriptsBase> allVersions, Version lastVersion, IMongoDatabase mongoDatabase)
  {
    var updatedToVersion = lastVersion;

    foreach (var version in allVersions.Where(a => a.Version > lastVersion))
    {
      await version.AfterScriptRunCode(mongoDatabase, _logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }

  private async Task<bool> DatabaseHasFirstUpdate()
  {
    var listCollections = await (await _mongoDatabase.ListCollectionsAsync()).ToListAsync();
    return listCollections.Count == 0;
  }

  private static (string Name, bool IsAuditable) GetColumnAuditAttrInfo<TEntity>(string propertyName)
  {
    var type = typeof(TEntity);
    return (type.GetMongoEntityName(propertyName), type.IsPropertyAuditable(propertyName));
  }
}