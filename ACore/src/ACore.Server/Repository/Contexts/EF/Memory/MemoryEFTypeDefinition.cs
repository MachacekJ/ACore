using ACore.Repository.Definitions.Models;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Repository.Contexts.EF.Memory;

public class MemoryEFTypeDefinition : EFTypeDefinition
{
  public override RepositoryTypeEnum Type => RepositoryTypeEnum.MemoryEF;
  public override string DataAnnotationColumnNameKey => string.Empty;
  public override string DataAnnotationTableNameKey => string.Empty;
  public override bool IsTransactionEnabled => false;

  public override Task<bool> DatabaseHasFirstUpdate<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<EFContextBase> logger)
  {
    return Task.FromResult(false);
  }
  
#pragma warning disable CS8602 // Dereference of a possibly null reference.
  protected override int CreatePKInt<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKIntEntity).Id) + 1;

  protected override long CreatePKLong<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKLongEntity).Id) + 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
  
}