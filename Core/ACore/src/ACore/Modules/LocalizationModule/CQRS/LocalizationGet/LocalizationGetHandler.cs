using ACore.Modules.LocalizationModule.CQRS.LocalizationGet.Models;
using ACore.Modules.LocalizationModule.Repositories;
using ACore.Results;
using MediatR;

namespace ACore.Modules.LocalizationModule.CQRS.LocalizationGet;

public class LocalizationGetHandler(ILocalizationRepository localizationRepository) : IRequestHandler<LocalizationGetQuery, Result<LocalizationGetQueryDataOut>>
{
  public Task<Result<LocalizationGetQueryDataOut>> Handle(LocalizationGetQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}