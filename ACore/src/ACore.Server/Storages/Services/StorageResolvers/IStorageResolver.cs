using ACore.Server.Storages.Definitions.Models;

namespace ACore.Server.Storages.Services.StorageResolvers;

public interface IStorageResolver
{
  Task ConfigureStorage<TStorage>(StorageImplementation implementation)
    where TStorage : IStorage;
  
  T ReadFromStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.All) where T : IStorage;
  IEnumerable<T> WriteToStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.All) where T : IStorage;
}