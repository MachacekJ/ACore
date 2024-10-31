using System.ComponentModel.DataAnnotations;
using Mapster;

namespace ACore.Server.Storages.Contexts.EF.Models.PK;

/// <summary>
/// Primary key of entity.
/// </summary>
public abstract class PKEntity<TPK>(TPK id)
{
  [Key]
  public TPK Id { get; set; } = id;

  protected static TEntity ToEntity<TEntity>(object data, TypeAdapterConfig? config = null)
    where TEntity : PKEntity<TPK>, new()
  {
    return config == null ? data.Adapt<TEntity>() : data.Adapt<TEntity>(config);
  }
}