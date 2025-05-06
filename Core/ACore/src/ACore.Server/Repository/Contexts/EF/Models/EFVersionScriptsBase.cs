using ACore.Repository;
using ACore.Server.Repository.Contexts.EF.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Repository.Contexts.EF.Models;

public abstract class EFVersionScriptsBase
{
  /// <summary>
  /// Version of update.
  /// </summary>
  public abstract Version Version { get; }

  /// <summary>
  /// Database script for update database.
  /// </summary>
  public virtual List<string> AllScripts { get; } = new();

  /// <summary>
  /// Call after <see cref="AllScripts" /> executed.
  /// Init data loading etc.
  /// </summary>
  public virtual void AfterScriptRunCode<T>(T dbContext, DbContextOptions options, IMediator mediator, ILogger<EFContextBase> logger) where T : IRepository
  {
  }
}