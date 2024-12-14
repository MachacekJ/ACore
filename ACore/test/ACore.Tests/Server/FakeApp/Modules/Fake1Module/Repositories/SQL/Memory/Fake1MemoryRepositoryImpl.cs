using ACore.Server.Repository.Contexts.EF.Base;
using ACore.Server.Repository.Contexts.EF.Memory;
using ACore.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Memory;

internal class Fake1MemoryRepositoryImpl(DbContextOptions<Fake1MemoryRepositoryImpl> options, IACoreServerCurrentScope serverCurrentScope, ILogger<Fake1SqlRepositoryImpl> logger)
  : Fake1SqlRepositoryImpl(options, serverCurrentScope, logger)
{
  protected override EFTypeDefinition EFTypeDefinition => new MemoryEFTypeDefinition();
}