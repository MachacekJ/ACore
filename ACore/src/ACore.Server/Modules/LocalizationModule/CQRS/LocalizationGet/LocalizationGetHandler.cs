using ACore.Results;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.LocalizationModule.CQRS.LocalizationGet.Models;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.LocalizationModule.CQRS.LocalizationGet;

public class LocalizationGetHandler(IRepositoryResolver repositoryResolver, IOptions<LocalizationServerModuleOptions> localizationServerModuleOptions) 
  : LocalizationModuleRequestHandler<LocalizationGetQuery, Result<LocalizationItemDataOut[]>>(repositoryResolver, localizationServerModuleOptions.Value)
{
  public override Task<Result<LocalizationItemDataOut[]>> Handle(LocalizationGetQuery request, CancellationToken cancellationToken)
  {
    var aa = ReadFromRepository().GetAllRecords(request.ContextId, request.Lcid);
    var res = aa.Select(i => new LocalizationItemDataOut(i.LocalizationKey, request.Lcid, i.Translation)).ToArray();
    return Task.FromResult(Result.Success(res));
  }
}