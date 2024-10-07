using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Definitions.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.MongoStorage;

public class MongoStorageDefinition : EFStorageDefinition
{
  private const string ErrorNotSupportedPK = $"Only PK {nameof(ObjectId)} is allowed for mongodb.";
  public override StorageTypeEnum Type => StorageTypeEnum.Mongo;
  public override string DataAnnotationColumnNameKey => "Mongo:ElementName";
  public override string DataAnnotationTableNameKey => "Mongo:CollectionName";
  public override bool IsTransactionEnabled => false;

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