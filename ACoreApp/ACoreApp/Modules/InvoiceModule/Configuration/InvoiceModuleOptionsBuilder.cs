using ACore.Server.Repository.Configuration;

namespace ACoreApp.Modules.InvoiceModule.Configuration;

public class InvoiceModuleOptionsBuilder : ServerRepositoryOptionBuilder
{
  public static InvoiceModuleOptionsBuilder Default() => new();

  public InvoiceModuleOptions Build(ServerRepositoryOptionBuilder defaultRepositories)
  {
    var defaultServerStorageOptions = defaultRepositories.Build();
    var res = new InvoiceModuleOptions(defaultServerStorageOptions);
    SetBase(res);
    return res;
  }
}