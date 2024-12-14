using ACore.Blazor;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageSave;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;

namespace ACoreApp.Client.UI.Pages;

public partial class Counter: ACorePageBase
{
  private int currentCount = 0;

  private async Task IncrementCount()
  {
    await Mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.Resources, "XXX", DateTime.Now, typeof(DateTime)));
    currentCount++;
  }
}