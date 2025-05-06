namespace ACore.Server.Repository.Contexts.EF.Models.PK;

/// <summary>
/// Primary key for sql db, not for mongo.
/// </summary>
public abstract class PKGuidEntity() : PKEntity<Guid>(EmptyId)
{
  public static Guid NewId => Guid.NewGuid();
  public static Guid EmptyId => Guid.Empty;
}