using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Server.Storages.Contexts.EF.FakeClasses;

public class FakeDbContextBaseImpl : DbContextBase
{
  public DbSet<FakeLongEntity> Fakes { get; set; }
  
  public FakeDbContextBaseImpl(DbContextOptions<FakeDbContextBaseImpl> options, IMediator mediator, ILogger<FakeDbContextBaseImpl> logger) : base(options, mediator, logger)
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