using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Definitions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ACore.Server.Storages.Definitions.EF;

public class MongoStorageDefinition : EFStorageDefinition
{
  private const string ErrorNotSupportedPK = $"Only PK {nameof(ObjectId)} is allowed for mongodb.";
  public override StorageTypeEnum Type => StorageTypeEnum.Mongo;
  public override string DataAnnotationColumnNameKey => "Mongo:ElementName";
  public override string DataAnnotationTableNameKey => "Mongo:CollectionName";
  public override bool IsTransactionEnabled => false;

  public override async Task<bool> DatabaseHasInitUpdate<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;
    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);
    var aa = await (await db.ListCollectionsAsync()).ToListAsync();
    return aa.Count == 0;
  }

  protected override int CreatePKInt<TEntity, TPK>(DbSet<TEntity> dbSet)
    => throw new NotImplementedException(ErrorNotSupportedPK);

  protected override long CreatePKLong<TEntity, TPK>(DbSet<TEntity> dbSet)
    => throw new NotImplementedException(ErrorNotSupportedPK);

  protected override Guid CreatePKGuid<TEntity, TPK>()
    => throw new NotImplementedException(ErrorNotSupportedPK);

  protected override string CreatePKString<TEntity, TPK>()
    => throw new NotImplementedException(ErrorNotSupportedPK);

  protected override ObjectId CreatePKObjectId<TEntity, TPK>()
    => PKMongoEntity.NewId;
}