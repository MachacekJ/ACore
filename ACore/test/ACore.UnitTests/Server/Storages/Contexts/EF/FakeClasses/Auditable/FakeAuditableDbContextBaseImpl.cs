using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.NotAuditable;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses.Auditable;

public class FakeAuditableDbContextBaseImpl : DbContextBase
{
  public DbSet<FakeAuditableLongEntity> Fakes { get; set; }
  
  public FakeAuditableDbContextBaseImpl(DbContextOptions<FakeAuditableDbContextBaseImpl> options, IMediator mediator, ILogger<FakeAuditableDbContextBaseImpl> logger) : base(options, mediator, logger)
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