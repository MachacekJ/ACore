using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Attributes;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using SampleServerPackage.ToDoModulePG.CQRS.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SampleServerPackage.ToDoModulePG.Repositories.SQL.Models;

[SumHash]
[Auditable(1)]
[Table("todo_list")]
[TableId("todo_list_id")]
internal class ToDoListEntity : PKIntEntity
{
  [Column("name")]
  [MaxLength(50)]
  public string Name { get; set; }

  [Column("created")]
  public DateTime Created { get; set; }

  public ICollection<ToDoListItemEntity> Items { get; set; }
  
  public static ToDoListEntity Create(ToDoListData data)
    => ToEntity<ToDoListEntity>(data);
}