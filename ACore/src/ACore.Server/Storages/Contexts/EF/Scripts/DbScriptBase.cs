namespace ACore.Server.Storages.Contexts.EF.Scripts;

public abstract class DbScriptBase
{
    /// <summary>
    /// For registration all change db scripts. 
    /// </summary>
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

