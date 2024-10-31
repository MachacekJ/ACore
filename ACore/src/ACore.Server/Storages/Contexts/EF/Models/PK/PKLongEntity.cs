namespace ACore.Server.Storages.Contexts.EF.Models.PK;

/// <summary>
/// Primary key for sql db, not for mongo.
/// </summary>
public abstract class PKLongEntity(): PKEntity<long>(EmptyId)
{
  public static long NewId => 0;
  public static long EmptyId => 0;
}