using ACore.Repository.Definitions.Models;

namespace ACore.Repository.Models;

public record RepositoryInfo(string ModuleName, RepositoryTypeEnum RepositoryType = RepositoryTypeEnum.Unknown)
{
}