using ACore.Server.Repository.Configuration;

namespace BlazorApp.Modules.InvoiceModule.Configuration;

public class InvoiceModuleOptions(ServerRepositoryOptions defaultServerRepositoryOptions) 
  : ServerRepositoryOptions(defaultServerRepositoryOptions, nameof(InvoiceModule));