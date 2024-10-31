using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;
using ScriptRegistrations = ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo.Scripts.ScriptRegistrations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Mongo;

internal class TestModuleMongoStorageImpl : DbContextBase, ITestStorageModule
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();

  internal DbSet<TestAuditEntity> Tests { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  
  public TestModuleMongoStorageImpl(DbContextOptions<TestModuleMongoStorageImpl> options, IMediator mediator, ILogger<TestModuleMongoStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(Tests);
    RegisterDbSet(TestValueTypes);
  }
  
  public async Task SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
    => await Save<TEntity, TPK>(data, hashToCheck);
  
  
  public async Task DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
    => await Delete<TEntity, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity, TPK>()  where TEntity : PKEntity<TPK>
  {
    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(TestAuditEntity) => Tests as DbSet<TEntity>,
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
  }
}