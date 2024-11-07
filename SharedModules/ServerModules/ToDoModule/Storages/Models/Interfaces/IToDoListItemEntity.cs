namespace ServerModules.ToDoModule.Storages.Models.Interfaces;

public interface IToDoListItemEntity<TPK, TPKList>
{
  public TPK Id { get; set; }
  public TPKList ToDoListId { get; set; }
  public decimal Amount { get; set; }
  public string Name { get; set; }
  public string? Description { get; set; }
  public decimal UnitPrice { get; set; }
}