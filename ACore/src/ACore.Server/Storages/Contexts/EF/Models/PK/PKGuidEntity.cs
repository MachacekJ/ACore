namespace ACore.Server.Storages.Contexts.EF.Models.PK;

public abstract class PKGuidEntity() : PKEntity<Guid>(EmptyId)
{
  public static Guid NewId => Guid.NewGuid();
  public static Guid EmptyId => Guid.Empty;
}