using ACore.Server.Services;
using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.UnitTests.Server.Storages.Contexts.EF.EventNotification.FakeClasses.NotAuditable;

public class FakeNotAuditableDbContextBaseImpl : DbContextBase
{
  public DbSet<FakeNotAuditableEntity> Fakes { get; set; }
  
  public FakeNotAuditableDbContextBaseImpl(DbContextOptions<FakeNotAuditableDbContextBaseImpl> options, IACoreServerApp mediator, ILogger<FakeNotAuditableDbContextBaseImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(Fakes);
  }

  protected override DbScriptBase UpdateScripts { get; } = new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition { get; } = new MemoryEFStorageDefinition();
  protected override string ModuleName => "FakeTestModule";
  
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
      optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
  }
}