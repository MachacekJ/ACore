using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.PG;
using ACore.Server.Services;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG.Scripts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.PG;

internal class Fake1PGRepositoryImpl(DbContextOptions<Fake1PGRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<Fake1PGRepositoryImpl> logger)
  : Fake1SqlRepositoryImpl(options, serverCurrentScope, logger)
{

  protected override EFTypeDefinition EFTypeDefinition => new PGTypeDefinition();
  protected override List<EFVersionScriptsBase> AllUpdateVersions { get; } = EFScriptRegistrations.AllVersions;
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    SetDatabaseNames<Fake1NoAuditEntity>(modelBuilder);
    SetDatabaseNames<Fake1AuditEntity>(modelBuilder);
    SetDatabaseNames<Fake1ValueTypeEntity>(modelBuilder);
    SetDatabaseNames<Fake1PKGuidEntity>(modelBuilder);
    SetDatabaseNames<Fake1PKStringEntity>(modelBuilder);
    SetDatabaseNames<Fake1PKLongEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}