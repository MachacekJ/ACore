namespace ACore.Repository.Definitions.Models;

[Flags]
public enum RepositoryTypeEnum
{
  Unknown = 0,
  First = 1 << 0,
  MemoryEF = 1 << 1,
  Postgres = 1 << 2,
  Mongo = 1 << 3,

  FileSystem = 1 << 4,
  Http = 1 << 5,

  AllDb = MemoryEF | Postgres | Mongo
}