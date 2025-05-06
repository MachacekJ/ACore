namespace ACore.Repository.Definitions.Models;

[Flags]
public enum RepositoryModeEnum
{
  Read = 1 << 0,
  Write = 1 << 1,
  ReadWrite = Read | Write
}