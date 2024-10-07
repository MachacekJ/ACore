using ACore.Server.Storages.Definitions.Models;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.PGStorage;

public class PGStorageDefinition : EFStorageDefinition
{
  public override StorageTypeEnum Type => StorageTypeEnum.Postgres;
  public override string DataAnnotationColumnNameKey => "Relational:ColumnName";
  public override string DataAnnotationTableNameKey => "Relational:TableName";
  public override bool IsTransactionEnabled => true;
  
  protected override ObjectId CreatePKObjectId<TEntity, TPK>()
    => throw new Exception($"PK {nameof(ObjectId)} is not allowed for postgres.");
}