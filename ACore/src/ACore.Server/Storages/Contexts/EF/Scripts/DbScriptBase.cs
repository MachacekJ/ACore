namespace ACore.Server.Storages.Contexts.EF.Scripts;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

