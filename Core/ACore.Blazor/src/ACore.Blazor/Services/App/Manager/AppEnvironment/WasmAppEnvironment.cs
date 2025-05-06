using System.Globalization;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageGet;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageSave;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using ACore.Blazor.Services.Javascript;
using MediatR;
using Microsoft.JSInterop;

namespace ACore.Blazor.Services.App.Manager.AppEnvironment;

public class WasmAppEnvironment(IMediator mediator, IJSRuntime jsRuntime) : IAppEnvironment
{
  public async Task SetStartLanguage(int defaultLanguage)
  {
    var languageFromLocalStorage = await mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.Resources, "currentLanguage"));

    if (languageFromLocalStorage.IsValue)
    {
      var lcid = languageFromLocalStorage.GetValue<int>();
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(lcid);
      CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(lcid);
      return;
    }

    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(defaultLanguage);
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(defaultLanguage);
  }


  public async Task ChangeLanguage(int lcid)
  {
    if (CultureInfo.DefaultThreadCurrentCulture?.LCID == lcid)
      return;

    var newCulture = new CultureInfo(lcid);
    CultureInfo.DefaultThreadCurrentCulture = newCulture;
    CultureInfo.DefaultThreadCurrentUICulture = newCulture;

    await jsRuntime.SetAspNetCoreCultureCookie(newCulture.Name);
    await mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.Resources, "currentLanguage", lcid, lcid.GetType()));
  }
}