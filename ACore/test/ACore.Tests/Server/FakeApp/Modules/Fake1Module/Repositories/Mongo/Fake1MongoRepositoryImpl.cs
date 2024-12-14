using ACore.Results;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Scripts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo;

// ReSharper disable once ClassNeverInstantiated.Global
internal class Fake1MongoRepositoryImpl : MongoContextBase, IFake1Repository
{
  private readonly IMongoCollection<Fake1AuditEntity> _testAudits;
  private readonly IMongoCollection<Fake1NoAuditEntity> _testNoAudits;
  private readonly IMongoCollection<Fake1ValueTypeEntity> _testValueTypeAudits;

  protected override string ModuleName => nameof(IFake1Repository);
  protected override IEnumerable<MongoVersionScriptsBase> AllUpdateVersions => MongoScriptRegistrations.AllVersions;


  public Fake1MongoRepositoryImpl(IACoreServerCurrentScope serverCurrentScope, IOptions<Fake1ModuleOptions> options, IMediator mediator, ILogger<Fake1MongoRepositoryImpl> logger)
    : base(serverCurrentScope, options.Value.MongoDb ?? throw new ArgumentNullException(nameof(options.Value.MongoDb)), mediator, logger)
  {
    _testAudits = MongoDatabase.GetCollection<Fake1AuditEntity>(typeof(Fake1AuditEntity).GetCollectionName());
    _testNoAudits = MongoDatabase.GetCollection<Fake1NoAuditEntity>(typeof(Fake1AuditEntity).GetCollectionName());
    _testValueTypeAudits = MongoDatabase.GetCollection<Fake1ValueTypeEntity>(typeof(Fake1AuditEntity).GetCollectionName());
  }

  public async Task<RepositoryOperationResult> SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK> =>
    data switch
    {
      Fake1AuditEntity fake1AuditEntity => await Save(_testAudits, fake1AuditEntity, hashToCheck),
      Fake1NoAuditEntity fake1NoAuditEntity => await Save(_testNoAudits, fake1NoAuditEntity, hashToCheck),
      Fake1ValueTypeEntity fake1ValueTypeEntity => await Save(_testValueTypeAudits, fake1ValueTypeEntity, hashToCheck),
      _ => RepositoryOperationResult.InternalError(new NotSupportedException())
    };

  public async Task<RepositoryOperationResult> DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    if (typeof(TEntity) == typeof(Fake1AuditEntity))
      return await Delete(_testAudits, (ObjectId)(Convert.ChangeType(id, typeof(ObjectId)) ?? throw new InvalidCastException()));

    if (typeof(TEntity) == typeof(Fake1NoAuditEntity))
      return await Delete(_testNoAudits, (ObjectId)(Convert.ChangeType(id, typeof(ObjectId)) ?? throw new InvalidCastException()));
    
    if (typeof(TEntity) == typeof(Fake1ValueTypeEntity))
      return await Delete(_testValueTypeAudits, (ObjectId)(Convert.ChangeType(id, typeof(ObjectId)) ?? throw new InvalidCastException()));
    
    return RepositoryOperationResult.InternalError(new NotSupportedException());
  }


  public async Task<Result<List<TEntity>>> GetAll<TEntity>()
    where TEntity : class
  {
    if (typeof(TEntity) == typeof(Fake1AuditEntity))
    {
      var filter = Builders<Fake1AuditEntity>.Filter.Empty;
      using var cursor = await _testAudits.FindAsync(filter);
      var allItems = await cursor.ToListAsync();
      var allItemsType = allItems.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }

    if (typeof(TEntity) == typeof(Fake1NoAuditEntity))
    {
      var filter = Builders<Fake1NoAuditEntity>.Filter.Empty;
      using var cursor = await _testNoAudits.FindAsync(filter);
      var allItems = await cursor.ToListAsync();
      var allItemsType = allItems.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }

    if (typeof(TEntity) == typeof(Fake1ValueTypeEntity))
    {
      var filter = Builders<Fake1ValueTypeEntity>.Filter.Empty;
      using var cursor = await _testValueTypeAudits.FindAsync(filter);
      var allItems = await cursor.ToListAsync();
      var allItemsType = allItems.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }

    return Result.Failure<List<TEntity>>(new NotSupportedException());
  }
}