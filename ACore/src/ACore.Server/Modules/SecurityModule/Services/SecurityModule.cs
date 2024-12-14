using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Modules.SecurityModule.Services;

public class SecurityModule(UserData currentUser) : ISecurityModule
{
  public UserData CurrentUser => currentUser;
}