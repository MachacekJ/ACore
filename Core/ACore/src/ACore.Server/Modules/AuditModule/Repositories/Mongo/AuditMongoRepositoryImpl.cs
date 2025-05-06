using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Repositories.Helpers;
using ACore.Server.Modules.AuditModule.Repositories.Mongo.Models;
using ACore.Server.Repository.Attributes.Extensions;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Contexts.Mongo.Models;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Server.Repository.Models.EntityEvent;
using ACore.Server.Repository.Results;
using ACore.Server.Repository.Results.Models;
using ACore.Server.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Server.Modules.AuditModule.Repositories.Mongo;

internal class AuditMongoRepositoryImpl : MongoContextBase, IAuditRepository
{
  protected override IEnumerable<MongoVersionScriptsBase> AllUpdateVersions => Scripts.MongoScriptRegistrations.AllVersions;
  protected override string ModuleName => nameof(IAuditRepository);

  private readonly IACoreServerCurrentScope _serverCurrentScope;
  private readonly IMongoCollection<AuditMongoEntity> _auditDbCollection;

  public AuditMongoRepositoryImpl(IACoreServerCurrentScope serverCurrentScope, IOptions<AuditModuleOptions> options, IMediator mediator, ILogger<AuditMongoRepositoryImpl> logger)
    : base(serverCurrentScope, options.Value.MongoDb ?? throw new ArgumentNullException(nameof(options.Value.MongoDb)), mediator, logger)
  {
    _serverCurrentScope = serverCurrentScope;
    _auditDbCollection = MongoDatabase.GetCollection<AuditMongoEntity>(typeof(AuditMongoEntity).GetCollectionName());
  }

  public async Task<RepositoryOperationResult> SaveAuditAsync(EntityEventItem entityEventItem)
  {
    if (entityEventItem.IsAuditable == false || !entityEventItem.ChangedColumns.Any())
      return RepositoryOperationResult.Success(RepositoryOperationTypeEnum.Unknown);

    var auditEntity = new AuditMongoEntity
    {
      Id = PKMongoEntity.EmptyId,
      ObjectId = GetObjectId(entityEventItem.TableName, new ObjectId(entityEventItem.PkValueString)),
      Version = entityEventItem.Version,
      User = new AuditMongoUserEntity
      {
        Id = entityEventItem.UserId
      },
      EntityState = entityEventItem.EntityState,
      Created = DateTime.UtcNow,
      Columns = entityEventItem.ChangedColumns.Where(e => e.IsAuditable).Select(e => new AuditMongoValueEntity
      {
        PropName = e.PropName,
        Property = e.ColumnName,
        DataType = e.DataType,
        IsChanged = e.IsChanged,
        // If the value is the same, the database may not be populated with the value. Saves space.
        NewValue = e.IsChanged ? e.NewValue.ToAuditValue() : null,
        OldValue = e.OldValue.ToAuditValue()
      }).ToList()
    };

    var res = await Save(_auditDbCollection, auditEntity);
    return res;
  }

  public async Task<AuditInfoItem[]> AuditItemsAsync<TPK>(string collectionName, TPK pkValue, string? schemaName = null)
  {
    if (pkValue == null)
      throw new Exception("Primary key is null");

    using var cursor = await _auditDbCollection.FindAsync(e => e.ObjectId == GetObjectId(collectionName, new ObjectId(pkValue.ToString())));
    var valuesTable = await cursor.ToListAsync();

    var ll = new List<AuditInfoItem>();
    foreach (var auditMongoEntity in valuesTable)
    {
      var auditInfoItem = new AuditInfoItem(collectionName, null, auditMongoEntity.Version, pkValue, auditMongoEntity.EntityState, auditMongoEntity.User.Id)
      {
        Created = auditMongoEntity.Created
      };

      if (auditMongoEntity.Columns != null)
      {
        foreach (var col in auditMongoEntity.Columns)
        {
          auditInfoItem.AddColumnEntry(new AuditInfoColumnItem(
            col.PropName, col.Property,
            col.DataType, col.IsChanged,
            col.OldValue.ConvertObjectToDataType(col.DataType),
            col.NewValue.ConvertObjectToDataType(col.DataType)
          ));
        }
      }

      ll.Add(auditInfoItem);
    }

    return ll.ToArray();
  }

  private static string GetObjectId(string collection, ObjectId pk) => $"{collection}.{pk}";
}