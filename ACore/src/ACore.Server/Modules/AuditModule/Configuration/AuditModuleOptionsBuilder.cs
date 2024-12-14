using ACore.Server.Repository.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  public static AuditModuleOptionsBuilder Empty() => new();
  public AuditModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    var defaultServerRepositoryOptions = defaultRepositories.Build();
    var res = new AuditModuleOptions(defaultServerRepositoryOptions);
    SetBase(res);
    return res;
  }
}