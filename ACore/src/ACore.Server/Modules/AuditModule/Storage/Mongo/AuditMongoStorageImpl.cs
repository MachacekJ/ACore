using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Helpers;
using ACore.Server.Modules.AuditModule.Storage.Mongo.Models;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Models.EntityEvent;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Server.Modules.AuditModule.Storage.Mongo;

internal class AuditMongoStorageImpl(DbContextOptions<AuditMongoStorageImpl> options, IMediator mediator, ILogger<AuditMongoStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditMongoEntity> Audits { get; set; }

  public async Task<DatabaseOperationResult> SaveAuditAsync(EntityEventItem entityEventItem)
  {
    if (entityEventItem.IsAuditable == false || !entityEventItem.ChangedColumns.Any())
      return DatabaseOperationResult.Success(DatabaseOperationTypeEnum.Unknown);

    var auditEntity = new AuditMongoEntity
    {
      Id = PKMongoEntity.NewId,
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

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
    return DatabaseOperationResult.Success(DatabaseOperationTypeEnum.Added);
  }

  public async Task<AuditInfoItem[]> AuditItemsAsync<TPK>(string collectionName, TPK pkValue, string? schemaName = null)
  {
    if (pkValue == null)
      throw new Exception("Primary key is null");

    var valuesTable = await Audits.Where(e => e.ObjectId == GetObjectId(collectionName, new ObjectId(pkValue.ToString()))).ToArrayAsync();
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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(DefaultNames.AuditCollectionName);
  }
}