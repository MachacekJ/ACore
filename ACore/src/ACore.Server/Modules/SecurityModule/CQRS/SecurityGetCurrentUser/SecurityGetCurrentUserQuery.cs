using ACore.Results;
using ACore.Server.Modules.SecurityModule.Models;

namespace ACore.Server.Modules.SecurityModule.CQRS.SecurityGetCurrentUser;

public class SecurityGetCurrentUserQuery : SecurityModuleRequest<Result<UserData>>;
  