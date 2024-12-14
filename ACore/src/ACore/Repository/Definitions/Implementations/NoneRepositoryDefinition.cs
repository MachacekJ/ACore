using ACore.Repository.Definitions.Models;

namespace ACore.Repository.Definitions.Implementations;

public class NoneRepositoryDefinition : RepositoryDefinition
{
  public override RepositoryTypeEnum Type => RepositoryTypeEnum.First;
}