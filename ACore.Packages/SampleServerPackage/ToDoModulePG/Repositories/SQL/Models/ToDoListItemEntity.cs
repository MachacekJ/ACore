using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Attributes;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SampleServerPackage.ToDoModulePG.Repositories.SQL.Models;

[SumHash]
[Auditable(1)]
[Table("todo_list_item")]
[TableId("todo_list_item_id")]
internal class ToDoListItemEntity : PKIntEntity
{
  [Column("todo_list_id")]
  public long ToDoListId { get; set; }

  [Column("amount")]
  [Precision(19, 4)]
  public decimal Amount { get; set; }

  [Column("name")]
  [MaxLength(50)]
  public string Name { get; set; }

  [Column("description")]
  public string? Description { get; set; }

  [Column("unit_price")]
  [Precision(19, 4)]
  public decimal UnitPrice { get; set; }

  [Column("created")]
  public DateTime Created { get; set; }

  [ForeignKey(nameof(ToDoListId))]
  public ToDoListEntity ToDoList { get; set; }
}