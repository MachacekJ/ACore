using ACore.Server.Storages.Contexts.EF;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Scripts;
using ACore.Server.Storages.Definitions.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampleServerPackage.ToDoModulePG.Repositories.SQL.Models;
using ScriptRegistrations = SampleServerPackage.ToDoModulePG.Repositories.SQL.Scripts.ScriptRegistrations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SampleServerPackage.ToDoModulePG.Repositories.SQL;

internal class ToDoRepositoryPGStorageImpl : DbContextBase, IToDoRepository
{
  internal DbSet<ToDoListEntity> ToDoList { get; set; }

  public ToDoRepositoryPGStorageImpl(DbContextOptions<ToDoRepositoryPGStorageImpl> options, IMediator mediator, ILogger<ToDoRepositoryPGStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(ToDoList);
  }

  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new PGStorageDefinition();
  protected override string ModuleName => nameof(IToDoRepository);


  public async Task<RepositoryOperationResult> SaveToDoList(ToDoListEntity toDoList, string? sumHash = null)
    => await Save<ToDoListEntity, int>(toDoList, sumHash);


  public async Task<RepositoryOperationResult> DeleteToDoList(int id)
    => await Delete<ToDoListEntity, int>(id);

  public async Task<ToDoListEntity?> DetailDetailToDoList(int id)
    => await GetEntityById<ToDoListEntity, int>(id);

  public async Task<IEnumerable<ToDoListEntity>> LoadToDoList()
    => await GetDbSet<ToDoListEntity>().ToListAsync();
}