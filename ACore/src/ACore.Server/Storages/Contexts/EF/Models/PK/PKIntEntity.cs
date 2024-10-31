namespace ACore.Server.Storages.Contexts.EF.Models.PK;

/// <summary>
/// Primary key for sql db, not for mongo.
/// </summary>
public abstract class PKIntEntity() : PKEntity<int>(EmptyId)
{
  public static int NewId => 0;
  public static int EmptyId => 0;
}