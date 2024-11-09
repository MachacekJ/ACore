using ACore.Server.Storages.Definitions.Models;

namespace SampleServerPackage.Tests.Tests.Modules.ToDoModule.CQRS;

public class CQRSTests() : ToDoModuleTestsBase([StorageTypeEnum.Postgres, StorageTypeEnum.Mongo])
{
  
}