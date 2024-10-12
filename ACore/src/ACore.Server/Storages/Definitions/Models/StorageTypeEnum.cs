namespace ACore.Server.Storages.Definitions.Models;

[Flags]
public enum StorageTypeEnum
{
  MemoryEF = 1 << 0,
  Postgres = 1 << 1,
  Mongo = 1 << 2,

  All = MemoryEF | Postgres | Mongo
}