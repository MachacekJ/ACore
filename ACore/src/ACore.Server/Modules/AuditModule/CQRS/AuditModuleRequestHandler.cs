using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS;

public abstract class AuditModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IStorageResolver _storageResolver = storageResolver;
  protected IAuditStorageModule ReadAuditContext() => _storageResolver.ReadFromStorage<IAuditStorageModule>();
}