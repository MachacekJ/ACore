using ACore.Blazor.Services.App.Manager.Extensions;
using ACore.Blazor.Services.App.Manager.Models.Actions;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ACore.Blazor.Services.App.Manager;

/// <summary>
/// Interface to manage app. This interface should be registered in DI and all pages and components can use manager.
/// Implementation should persist information about basic app state.
/// e.g. responsive type, current page
/// </summary>
public interface IAppManager
{
  /// <summary>
  /// It will be called if <see cref="ResponsiveType"/> is changed. Components can adapt to the new resolution
  /// </summary>
  event Func<ResponsiveTypeEnum, Task> OnResponsiveChange;

  #region Properties

  RightMenuActions RightMenu { get; }
  PageActions Page { get; }
  //LocalizationActions Localization { get; }
  ResponsiveTypeEnum ResponsiveType { get; }
  IAppSettings AppSettings { get; }
  IAppEnvironment AppEnvironment { get; }
  IJSRuntime? JsRuntime { get; }
  #endregion

  #region Methods

  /// <summary>
  /// Cannot use classic DI, because <see cref="NavigationManager"/> is not implemented correctly.
  /// </summary>
  /// <param name="navigationManager"> must be obtained from any component.</param>
  /// <param name="renderMode">m</param>
  Task Init(NavigationManager navigationManager, IJSRuntime jsRuntime, string renderMode);

  /// <summary>
  /// Set new responsive type, e.g. change size of browser. 
  /// </summary>
  void SetResponsiveType(ResponsiveTypeEnum responsiveType);

  T GetExtension<T>() where T : IAppExtension;
  #endregion
}