using ACore.Results;
using ACore.Server.Repository.Contexts.EF;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Results;
using ACore.Server.Services;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL;

internal abstract class Fake1SqlRepositoryImpl : EFContextBase, IFake1Repository
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => [];
  protected override string ModuleName => nameof(IFake1Repository);

  internal DbSet<Fake1NoAuditEntity> TestNoAudits { get; set; }
  internal DbSet<Fake1AuditEntity> TestAudits { get; set; }
  internal DbSet<Fake1ValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<Fake1PKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<Fake1PKStringEntity> TestPKString { get; set; }
  internal DbSet<Fake1PKLongEntity> TestPKLong { get; set; }

  protected Fake1SqlRepositoryImpl(DbContextOptions options, IACoreServerCurrentScope serverCurrentScope, ILogger<Fake1SqlRepositoryImpl> logger) : base(options, serverCurrentScope, logger)
  {
    RegisterDbSet(TestNoAudits);
    RegisterDbSet(TestAudits);
    RegisterDbSet(TestValueTypes);
    RegisterDbSet(TestPKGuid);
    RegisterDbSet(TestPKString);
    RegisterDbSet(TestPKLong);
  }

  public async Task<RepositoryOperationResult> SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
    => await Save<TEntity, TPK>(data, hashToCheck);

  public async Task<RepositoryOperationResult> DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
    => await Delete<TEntity, TPK>(id);

  public async Task<Result<List<TEntity>>> GetAll<TEntity>() where TEntity : class
  {
    if (typeof(TEntity) == typeof(Fake1AuditEntity))
    {
      var allItems = await GetDbSet<Fake1AuditEntity>().ToListAsync();
      var allItemsType = allItems.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }

    if (typeof(TEntity) == typeof(Fake1NoAuditEntity))
    {
      var allItems = await GetDbSet<Fake1NoAuditEntity>().ToListAsync();
      var allItemsType = allItems.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }

    if (typeof(TEntity) == typeof(Fake1ValueTypeEntity))
    {
      var db = await GetDbSet<Fake1ValueTypeEntity>().ToListAsync();
      var allItemsType = db.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }
    
    if (typeof(TEntity) == typeof(Fake1PKStringEntity))
    {
      var db = await GetDbSet<Fake1PKStringEntity>().ToListAsync();
      var allItemsType = db.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }
    
    if (typeof(TEntity) == typeof(Fake1PKLongEntity))
    {
      var db = await GetDbSet<Fake1PKLongEntity>().ToListAsync();
      var allItemsType = db.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }
    
    if (typeof(TEntity) == typeof(Fake1PKGuidEntity))
    {
      var db = await GetDbSet<Fake1PKGuidEntity>().ToListAsync();
      var allItemsType = db.ConvertAll(e => (TEntity)Convert.ChangeType(e, typeof(TEntity)));
      return Result.Success(allItemsType);
    }
    
    return Result.Failure<List<TEntity>>(new NotSupportedException());
  }
}