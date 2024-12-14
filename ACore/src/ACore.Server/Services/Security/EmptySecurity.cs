using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Services.Security;

public class EmptySecurity : ISecurity
{
  public UserData CurrentUser => new(UserTypeEnum.System, string.Empty, string.Empty);
}