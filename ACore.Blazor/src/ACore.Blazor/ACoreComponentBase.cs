using ACore.Blazor.Modules.LoadingModule;
using ACore.Blazor.Modules.LocalizationModule;
using ACore.Blazor.Services.App.Manager;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace ACore.Blazor;

public abstract class ACoreComponentBase : ComponentBase, IDisposable
{
  protected virtual bool LocalizationEnabled => false;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  protected LocalizationAppExtension LocalizationAppExtension { get; private set; }
  protected LoadingAppExtension LoadingAppExtension { get; private set; }
  
  [Inject]
  public required IMediator Mediator { get; set; }

  [Inject]
  public required IAppManager AppManager { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  
  protected override void OnInitialized()
  {
    LocalizationAppExtension = AppManager.GetExtension<LocalizationAppExtension>();
    LoadingAppExtension = AppManager.GetExtension<LoadingAppExtension>();
    
    LocalizationAppExtension.OnLocalizationChangeAsync += LocalizationChangeAsync;
  }

  private Task LocalizationChangeAsync()
  {
    if (LocalizationEnabled)
      StateHasChanged();
    return Task.CompletedTask;
  }

  public virtual void Dispose()
  {
    LocalizationAppExtension.OnLocalizationChangeAsync -= LocalizationChangeAsync;
  }
}