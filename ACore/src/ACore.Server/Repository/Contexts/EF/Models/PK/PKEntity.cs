using System.ComponentModel.DataAnnotations;
using Mapster;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Repository.Contexts.EF.Models.PK;

/// <summary>
/// Primary key of entity.
/// </summary>
public abstract class PKEntity<TPK>(TPK id)
 // where TPK : notnull
{
  [Key]
  [BsonElement("_id")]
  public TPK Id { get; set; } = id;

  protected static TEntity ToEntity<TEntity>(object data, TypeAdapterConfig? config = null)
    where TEntity : PKEntity<TPK>, new()
  {
    return config == null ? data.Adapt<TEntity>() : data.Adapt<TEntity>(config);
  }
}