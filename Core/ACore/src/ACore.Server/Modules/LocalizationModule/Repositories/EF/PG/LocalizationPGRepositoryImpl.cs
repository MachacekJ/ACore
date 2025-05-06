using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Models;
using ACore.Server.Repository.Contexts.EF.PG;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.LocalizationModule.Repositories.EF.PG;

internal class LocalizationPGRepositoryImpl(DbContextOptions options, IACoreServerCurrentScope serverCurrentScope, ILogger<LocalizationSqlRepositoryImpl> logger)
  : LocalizationSqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override List<EFVersionScriptsBase> AllUpdateVersions => Scripts.EFScriptRegistrations.AllVersions;
  protected override EFTypeDefinition EFTypeDefinition => new PGTypeDefinition();
}