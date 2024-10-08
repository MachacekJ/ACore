using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Helpers;
using ACore.Server.Modules.AuditModule.Storage.Mongo.Models;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.MongoStorage;
using ACore.Server.Storages.Models.SaveInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.Modules.AuditModule.Storage.Mongo;

internal class AuditMongoStorageImpl(DbContextOptions<AuditMongoStorageImpl> options, IMediator mediator, ILogger<AuditMongoStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();
  protected override string ModuleName => nameof(IAuditStorageModule);

  // ReSharper disable once UnusedAutoPropertyAccessor.Global
  public DbSet<AuditMongoEntity> Audits { get; set; }

  public async Task SaveAuditAsync(SaveInfoItem saveInfoItem)
  {
    if (saveInfoItem.IsAuditable == false || !saveInfoItem.ChangedColumns.Any())
      return;

    var auditEntity = new AuditMongoEntity
    {
      Id = PKMongoEntity.NewId,
      ObjectId = GetObjectId(saveInfoItem.TableName, new ObjectId(saveInfoItem.PkValueString)),
      Version = saveInfoItem.Version,
      User = new AuditMongoUserEntity
      {
        Id = saveInfoItem.UserId
      },
      EntityState = saveInfoItem.EntityState,
      Created = DateTime.UtcNow,
   //   Columns = []
      Columns = saveInfoItem.ChangedColumns.Where(e => e.IsAuditable).Select(e => new AuditMongoValueEntity
      {
        PropName = e.PropName,
        Property = e.ColumnName,
        DataType = e.DataType,
        IsChanged = e.IsChanged,
        NewValue = e.NewValue.ToAuditValue(),
        OldValue = e.OldValue.ToAuditValue(),
      }).ToList()
    };

    // foreach (var column in saveInfoItem.ChangedColumns.Where(e => e.IsAuditable))
    // {
    //   auditEntity.Columns.Add(new AuditMongoValueEntity
    //   {
    //     PropName = column.PropName,
    //     Property = column.ColumnName,
    //     DataType = column.DataType,
    //     IsChanged = column.IsChanged,
    //     NewValue = column.NewValue.ToAuditValue(),
    //     OldValue = column.OldValue.ToAuditValue(),
    //   });
    // }

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  public async Task<AuditInfoItem[]> AuditItemsAsync<TPK>(string collectionName, TPK pkValue, string? schemaName = null)
  {
    if (pkValue == null)
      throw new Exception("Primary key is null");

    var valuesTable = await Audits.Where(e => e.ObjectId == GetObjectId(collectionName, new ObjectId(pkValue.ToString()))).ToArrayAsync();
    var ll = new List<AuditInfoItem>();
    foreach (var auditMongoEntity in valuesTable)
    {
      var aa = new AuditInfoItem(collectionName, null, auditMongoEntity.Version, pkValue, auditMongoEntity.EntityState, auditMongoEntity.User.Id);
      aa.Created = auditMongoEntity.Created;

      if (auditMongoEntity.Columns != null)
      {
        foreach (var col in auditMongoEntity.Columns)
        {
          aa.AddColumnEntry(new AuditInfoColumnItem(
            col.PropName, col.Property,
            col.DataType, col.IsChanged,
            col.OldValue.ConvertObjectToDataType(col.DataType),
            col.NewValue.ConvertObjectToDataType(col.DataType)
          ));
        }
      }

      ll.Add(aa);
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