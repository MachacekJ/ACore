using ACore.Repository.Definitions.Models;
using ACore.Server.Repository.Contexts.EF.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Repository.Contexts.EF.PG;

public class PGTypeDefinition : EFTypeDefinition
{
  public override RepositoryTypeEnum Type => RepositoryTypeEnum.Postgres;
  public override string DataAnnotationColumnNameKey => "Relational:ColumnName";
  public override string DataAnnotationTableNameKey => "Relational:TableName";
  public override bool IsTransactionEnabled => true;

  public override async Task<bool> DatabaseHasFirstUpdate<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<EFContextBase> logger)
  {
    var sql = "select count(*) as C from information_schema.tables where table_schema = 'public'";
    var res = await dbContext.Database.SqlQueryRaw<int>(sql).ToListAsync();
    if (res.Count == 0)
      return true;
    return res.First() == 0;
  }
}