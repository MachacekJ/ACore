using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using MediatR;
using SampleServerPackage.ToDoModulePG.Repositories;

namespace SampleServerPackage.ToDoModulePG.CQRS;

internal abstract class ToDoModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : StorageRequestHandler<TRequest, TResponse>(storageResolver)
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  private readonly IStorageResolver _storageResolver = storageResolver;
  protected IToDoRepository ReadTestContext() => _storageResolver.ReadFromStorage<IToDoRepository>();

  protected IToDoRepository WriteStorageContext => _storageResolver.WriteToStorages<IToDoRepository>().Single() ?? throw new Exception($"Check amount of registreg storage for {nameof(IToDoRepository)}");
}