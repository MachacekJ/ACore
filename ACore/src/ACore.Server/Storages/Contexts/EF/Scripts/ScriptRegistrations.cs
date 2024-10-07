namespace ACore.Server.Storages.Contexts.EF.Scripts;

public class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions => new List<DbVersionScriptsBase>();
}