using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.Contexts.EF.Scripts;

public abstract class DbVersionScriptsBase
{
    public abstract Version Version { get; }
    public virtual List<string> AllScripts { get; } = new();

    public virtual void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger) where T : IStorage
    {
      
    }
}