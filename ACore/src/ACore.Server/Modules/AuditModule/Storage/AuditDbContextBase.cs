using ACore.Server.Storages.Contexts.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Storage;

public abstract class AuditDbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger) : DbContextBase(options, mediator, logger)
{

}