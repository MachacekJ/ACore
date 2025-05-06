using ACore.Modules.LocalizationModule.CQRS.LocalizationGet.Models;
using ACore.Results;

namespace ACore.Modules.LocalizationModule.CQRS.LocalizationGet;

public class LocalizationGetQuery(Type localizationType): LocalizationModuleRequest<Result<LocalizationGetQueryDataOut>>
{
  public Type LocalizationType => localizationType;
}