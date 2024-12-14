using ACore.Server.Repository.Contexts.EF;
using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Memory;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification.FakeClasses.NotAuditable;

public class FakeNotAuditableEfContextBaseImpl : EFContextBase
{
  public DbSet<FakeNotAuditableEntity> Fakes { get; set; }
  
  public FakeNotAuditableEfContextBaseImpl(DbContextOptions<FakeNotAuditableEfContextBaseImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<FakeNotAuditableEfContextBaseImpl> logger) : base(options, serverCurrentScope, logger)
  {
    RegisterDbSet(Fakes);
  }

  protected override List<EFVersionScriptsBase> AllUpdateVersions => [];
  protected override EFTypeDefinition EFTypeDefinition { get; } = new MemoryEFTypeDefinition();
  protected override string ModuleName => "FakeTestModule";
  
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
      optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
  }
}