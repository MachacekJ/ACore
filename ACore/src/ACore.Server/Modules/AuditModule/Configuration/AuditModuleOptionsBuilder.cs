using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptionsBuilder : StorageModuleOptionBuilder
{
  public static AuditModuleOptionsBuilder Empty() => new();
  
  public AuditModuleOptionsBuilder Storages(Action<StorageOptionBuilder> action)
  {
    StoragesBase(action);
    return this;
  }

  public AuditModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new AuditModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(IAuditStorageModule)),
    };
  }
}