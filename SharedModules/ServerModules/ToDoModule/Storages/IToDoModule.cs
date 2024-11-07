using ACore.Server.Storages.Contexts.EF.Models;
using ServerModules.ToDoModule.Storages.Models.Interfaces;

namespace ServerModules.ToDoModule.Storages;

public interface IToDoModule
{
  Task<DatabaseOperationResult> SaveToDoList<TPK, TPKList>(IToDoListEntity<TPK, TPKList> toDoList, string? sumHash = null);
  Task<DatabaseOperationResult> DeleteToDoList<TPK>(TPK id);
  Task<IToDoListEntity<TPK, TPKList>> DetailDetailToDoList<TPK, TPKList>(TPK id);
  Task<List<IToDoListEntity<TPK, TPKList>>?> LoadToDoList<TPK, TPKList>(TPK id);
}