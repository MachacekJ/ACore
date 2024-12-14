using ACore.Repository;

namespace ACore.Server.Repository;

public interface IDbRepository : IRepository
{
  Task UpSchema();
}