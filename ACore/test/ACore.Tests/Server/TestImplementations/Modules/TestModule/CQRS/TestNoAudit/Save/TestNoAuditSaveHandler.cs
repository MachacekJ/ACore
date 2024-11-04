using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.SQL.Models;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand request, CancellationToken cancellationToken)
  {
    return await StorageEntityParallelAction((storage) =>
    {
      switch (storage)
      {
        case TestModuleSqlStorageImpl:
          var en = TestNoAuditEntity.Create(request.Data);
          return new StorageEntityExecutorItem<TestNoAuditEntity>(en, storage, storage.SaveTestEntity<TestNoAuditEntity, int>(en, request.SumHash));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}