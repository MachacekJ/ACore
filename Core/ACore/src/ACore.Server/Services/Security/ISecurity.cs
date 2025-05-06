using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Services.Security;

public interface ISecurity
{
  UserData CurrentUser { get; }
}