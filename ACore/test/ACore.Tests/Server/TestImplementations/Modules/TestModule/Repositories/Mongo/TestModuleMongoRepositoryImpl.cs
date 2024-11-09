using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;
using ScriptRegistrations = ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Scripts.ScriptRegistrations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo;

internal class TestModuleMongoRepositoryImpl : DbContextBase, ITestRepositoryModule
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestRepositoryModule);
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();

  internal DbSet<TestNoAuditEntity> TestNoAudits { get; set; }
  internal DbSet<TestAuditEntity> TestAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  
  public TestModuleMongoRepositoryImpl(DbContextOptions<TestModuleMongoRepositoryImpl> options, IMediator mediator, ILogger<TestModuleMongoRepositoryImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(TestNoAudits);
    RegisterDbSet(TestAudits);
    RegisterDbSet(TestValueTypes);
  }
  
  public async Task<RepositoryOperationResult> SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
    => await Save<TEntity, TPK>(data, hashToCheck);
  
  
  public async Task<RepositoryOperationResult> DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
    => await Delete<TEntity, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity, TPK>()  where TEntity : PKEntity<TPK>
  {
    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(TestAuditEntity) => TestAudits as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestNoAuditEntity) => TestNoAudits as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestValueTypeEntity) => TestValueTypes as DbSet<TEntity>,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"DbSet function returned null value.");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestAuditEntity>().ToCollection(DefaultNames.ObjectNameMapping[nameof(TestAuditEntity)].TableName);
    modelBuilder.Entity<TestAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestAuditEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
    
    modelBuilder.Entity<TestValueTypeEntity>().ToCollection(DefaultNames.ObjectNameMapping[nameof(TestValueTypeEntity)].TableName);
    modelBuilder.Entity<TestValueTypeEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestValueTypeEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
    
    modelBuilder.Entity<TestNoAuditEntity>().ToCollection(DefaultNames.ObjectNameMapping[nameof(TestNoAuditEntity)].TableName);
    modelBuilder.Entity<TestNoAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestNoAuditEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
  }
}