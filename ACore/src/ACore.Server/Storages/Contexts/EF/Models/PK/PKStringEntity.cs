namespace ACore.Server.Storages.Contexts.EF.Models.PK;

/// <summary>
/// Primary key for sql db, not for mongo.
/// </summary>
public abstract class PKStringEntity() : PKEntity<string>(EmptyId)
{
  public static string NewId => Guid.NewGuid().ToString();
  public static string EmptyId => string.Empty;
}