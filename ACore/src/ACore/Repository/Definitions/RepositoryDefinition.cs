using ACore.Repository.Definitions.Models;

namespace ACore.Repository.Definitions;

public abstract class RepositoryDefinition
{
  public abstract RepositoryTypeEnum Type { get; }
}