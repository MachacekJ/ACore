namespace ServerModules.ToDoModule.Storages.Models.Interfaces;

public interface IToDoListEntity<TPK, TPKList>
{
  public TPK Id { get; set; }
  public string Name { get; set; }
  public ICollection<IToDoListItemEntity<TPK, TPKList>> Items { get; set; }
}