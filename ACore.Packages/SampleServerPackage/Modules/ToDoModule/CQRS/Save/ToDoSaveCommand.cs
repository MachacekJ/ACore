using ACore.Models.Result;
using SampleServerPackage.ToDoModulePG.CQRS.Models;

namespace SampleServerPackage.ToDoModulePG.CQRS.Save;

public class ToDoSaveCommand(ToDoListData data): ToDoModuleRequest<Result>
{
  public ToDoListData Data => data;
}

