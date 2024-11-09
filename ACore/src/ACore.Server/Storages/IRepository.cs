using ACore.Server.Storages.Models;

namespace ACore.Server.Storages;

public interface IRepository
{
  RepositoryInfo RepositoryInfo { get; }
  Task UpSchema();
}