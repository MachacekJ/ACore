using ACore.Server.Storages;
using ACore.Server.Storages.Contexts.EF.Models;
using SampleServerPackage.ToDoModulePG.Repositories.SQL.Models;

namespace SampleServerPackage.ToDoModulePG.Repositories;

internal interface IToDoRepository : IRepository
{
  Task<RepositoryOperationResult> SaveToDoList(ToDoListEntity toDoList, string? sumHash = null);
  Task<RepositoryOperationResult> DeleteToDoList(int id);
  Task<ToDoListEntity?> DetailDetailToDoList(int id);
  Task<IEnumerable<ToDoListEntity>> LoadToDoList();
}