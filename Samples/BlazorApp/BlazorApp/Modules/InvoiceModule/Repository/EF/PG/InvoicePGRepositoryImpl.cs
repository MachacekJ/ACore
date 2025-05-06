using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.PG;
using ACore.Server.Services;
using BlazorApp.Modules.InvoiceModule.Repository.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Modules.InvoiceModule.Repository.EF.PG;

internal class InvoicePGRepositoryImpl(DbContextOptions<InvoicePGRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<InvoiceSqlRepositoryImpl> logger) 
  : InvoiceSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => Scripts.EFScriptRegistrations.AllVersions;
  protected override EFTypeDefinition EFTypeDefinition => new PGTypeDefinition();


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<InvoiceEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<InvoiceItemEntity>().HasKey(p => p.Id);

    SetDatabaseNames<InvoiceEntity>(modelBuilder);
    SetDatabaseNames<InvoiceItemEntity>(modelBuilder);
  }

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}