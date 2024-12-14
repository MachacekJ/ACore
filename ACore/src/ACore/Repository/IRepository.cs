using ACore.Repository.Models;

namespace ACore.Repository;

public interface IRepository
{
  RepositoryInfo RepositoryInfo { get; }
}