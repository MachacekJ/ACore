using ACore.Modules.LocalizationModule.Repositories;
using ACore.Results;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Repository.CQRS.Handlers;
using ACore.Server.Repository.Services.RepositoryResolvers;
using MediatR;

namespace ACore.Server.Modules.LocalizationModule.CQRS;

public abstract class LocalizationModuleRequestHandler<TRequest, TResponse>(IRepositoryResolver repositoryResolver, LocalizationServerModuleOptions localizationModuleOption) : RepositoryRequestHandler<ILocalizationRepository, TRequest, TResponse>(repositoryResolver, localizationModuleOption)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
 
}