using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Modules.SecurityModule.Services;

public interface ISecurityModule
{
  UserData CurrentUser { get; }
}