using ACore.Server.Repository.Configuration;

namespace ACoreApp.Modules.InvoiceModule.Configuration;

public class InvoiceModuleOptions(ServerRepositoryOptions defaultServerRepositoryOptions) 
  : ServerRepositoryOptions(defaultServerRepositoryOptions, nameof(InvoiceModule));