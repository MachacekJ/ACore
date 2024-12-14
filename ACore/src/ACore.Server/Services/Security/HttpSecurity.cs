using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Services.Security;

public class HttpSecurity : ISecurity
{
  public UserData CurrentUser => throw new NotImplementedException();
}