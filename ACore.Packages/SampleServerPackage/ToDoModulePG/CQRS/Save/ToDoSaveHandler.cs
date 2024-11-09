using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Results;
using ACore.Server.Storages.CQRS.Results.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Services.StorageResolvers;
using SampleServerPackage.ToDoModulePG.Repositories.SQL.Models;

namespace SampleServerPackage.ToDoModulePG.CQRS.Save;

internal class ToDoSaveHandler(IStorageResolver storageResolver)
  : ToDoModuleRequestHandler<ToDoSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(ToDoSaveCommand request, CancellationToken cancellationToken)
  {
    var writeStorage = WriteStorageContext;
    var en = ToDoListEntity.Create(request.Data);
    var res = await writeStorage.SaveToDoList(en);
    var r = EntityResult.SuccessWithValues(new Dictionary<RepositoryInfo, EntityResultData>
    {
      { writeStorage.RepositoryInfo, new EntityResultData(en.Id, res) }
    });
    return r;
  }
}