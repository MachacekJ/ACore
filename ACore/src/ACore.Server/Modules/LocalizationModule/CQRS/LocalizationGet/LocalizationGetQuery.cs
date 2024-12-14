using ACore.Results;
using ACore.Server.Modules.LocalizationModule.CQRS.LocalizationGet.Models;

namespace ACore.Server.Modules.LocalizationModule.CQRS.LocalizationGet;

public class LocalizationGetQuery(Type contextId, int lcid) : LocalizationModuleRequest<Result<LocalizationItemDataOut[]>>
{
  public Type ContextId => contextId;
  public int Lcid => lcid;
}
