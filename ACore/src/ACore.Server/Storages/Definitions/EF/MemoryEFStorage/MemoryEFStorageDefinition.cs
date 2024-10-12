using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Definitions.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.MemoryEFStorage;

public class MemoryEFStorageDefinition : EFStorageDefinition
{
  public override StorageTypeEnum Type => StorageTypeEnum.MemoryEF;
  public override string DataAnnotationColumnNameKey => string.Empty;
  public override string DataAnnotationTableNameKey => string.Empty;
  public override bool IsTransactionEnabled => false;
  
#pragma warning disable CS8602 // Dereference of a possibly null reference.
  protected override int CreatePKInt<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKIntEntity).Id) + 1;

  protected override long CreatePKLong<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKLongEntity).Id) + 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
  
  protected override ObjectId CreatePKObjectId<TEntity, TPK>()
    => throw new Exception($"PK {nameof(ObjectId)} is not allowed for memory EF.");
}