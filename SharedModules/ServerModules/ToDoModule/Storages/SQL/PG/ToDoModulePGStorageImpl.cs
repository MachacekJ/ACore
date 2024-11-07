using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerModules.ToDoModule.Storages.Models.Interfaces;

namespace ServerModules.ToDoModule.Storages.SQL.PG;

public class ToDoModulePGStorageImpl(DbContextOptions<ToDoModulePGStorageImpl> options, IMediator mediator, ILogger<ToDoModulePGStorageImpl> logger) : DbContextBase(options, mediator, logger), IToDoModule
{
  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new PGStorageDefinition();
  protected override string ModuleName => nameof(IToDoModule);
  
  public Task<DatabaseOperationResult> SaveToDoList<TPK, TPKList>(IToDoListEntity<TPK, TPKList> toDoList, string? sumHash = null)
  {
    throw new NotImplementedException();
  }

  public Task<DatabaseOperationResult> DeleteToDoList<TPK>(TPK id)
  {
    throw new NotImplementedException();
  }

  public Task<IToDoListEntity<TPK, TPKList>> DetailDetailToDoList<TPK, TPKList>(TPK id)
  {
    throw new NotImplementedException();
  }

  public Task<List<IToDoListEntity<TPK, TPKList>>?> LoadToDoList<TPK, TPKList>(TPK id)
  {
    throw new NotImplementedException();
  }
}

