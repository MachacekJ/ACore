using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS.Handlers;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand request, CancellationToken cancellationToken)
  {
    // var saltForHash = (await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt), cancellationToken)).ResultValue
    //                   ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");
    return await PerformWriteActionWithData((storage) =>
    {
      switch (storage)
      {
        case TestModuleSqlStorageImpl:
          var en = TestNoAuditEntity.Create(request.Data);
          return new SaveProcessExecutor<TestNoAuditEntity>(en, storage, storage.SaveTestEntity<TestNoAuditEntity, int>(en, request.Hash));
        default:
          throw new Exception($"Storage for '{storage.GetType()}' is not supported.");
      }
    });
  }
}