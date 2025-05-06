using ACore.Blazor.Modules.LocalizationModule;
using ACore.Blazor.Services.App.Manager.AppEnvironment;
using ACore.Blazor.Services.App.Manager.Models.Actions;
using ACore.Blazor.Services.App.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace ACore.Blazor.Services.App.Manager;

// NavigationManager cannot be in ctor. It must be used Init.
public class AppManager : IAppManager
{
  private readonly Dictionary<Type, Extensions.IAppExtension> _appManagerExtensions = [];
  private readonly ILogger<AppManager> _logger;
  private readonly IMediator _mediator;

  public AppManager(IAppSettings appSettings, IMediator mediator, ILogger<AppManager> logger)
  {
    _mediator = mediator;
    _logger = logger;
    Page = new PageActions(appSettings, null, logger);
    AppSettings = appSettings;
    RegisterAllAppExtensions(appSettings);
  }

  public event Func<ResponsiveTypeEnum, Task>? OnResponsiveChange;
  public RightMenuActions RightMenu { get; } = new();
  public PageActions Page { get; private set; }
  public IAppSettings AppSettings { get; }

  public ResponsiveTypeEnum ResponsiveType { get; private set; } = ResponsiveTypeEnum.Desktop;

  public IAppEnvironment AppEnvironment { get; private set; } = new StaticAppEnvironment();
  public IJSRuntime? JsRuntime { get; private set; }
  
  public Task Init(NavigationManager navigationManager, IJSRuntime jsRuntime, string renderMode)
  {
    AppEnvironment = renderMode.ToLower() switch
    {
      "static" => throw new NotImplementedException(),
      "server" => new ServerAppEnvironment(navigationManager),
      "webassembly" => new WasmAppEnvironment(_mediator, jsRuntime),
      "webview" => throw new NotImplementedException(),
      _ => throw new ArgumentOutOfRangeException()
    };
    JsRuntime = jsRuntime;
    Page = new PageActions(AppSettings, navigationManager, _logger);
    
    return Task.CompletedTask;
  }

  public void SetResponsiveType(ResponsiveTypeEnum responsiveType)
  {
    if (ResponsiveType == responsiveType)
      return;

    ResponsiveType = responsiveType;

    if (Page.PageState != PageStateEnum.Rendered)
      return;

    OnResponsiveChange?.Invoke(ResponsiveType);
  }

  public T GetExtension<T>() where T : Extensions.IAppExtension
  {
    if (!_appManagerExtensions.TryGetValue(typeof(T), out var extension))
      throw new InvalidOperationException($"Extension of type {typeof(T).FullName} not found.");

    if (extension is not T res)
      throw new Exception("Extension of type " + typeof(T).FullName + " is not supported.");

    return res;
  }

  private void RegisterAllAppExtensions(IAppSettings appSettings)
  {
    _appManagerExtensions.Add(typeof(LocalizationAppExtension), new LocalizationAppExtension(this));

    var theInterface = typeof(Extensions.IAppExtension);
    foreach (var assembly in appSettings.Assemblies)
    {
      var allComponents = assembly.GetTypes().Where(t => theInterface.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false, IsClass: true }); // Search for types that implement the interface
      foreach (var type in allComponents)
      {
        if (_appManagerExtensions.ContainsKey(type))
          continue;

        if (Activator.CreateInstance(type) is Extensions.IAppExtension pageConfig)
          _appManagerExtensions.Add(type, pageConfig);
      }
    }
  }
}