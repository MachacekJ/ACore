using ACore.Blazor.Modules.Analytics;
using ACore.Blazor.Modules.Analytics.Models;
using ACore.Blazor.Services.App.Manager;
using ACore.Blazor.Services.App.Manager.Extensions;
using MediatR;
using Microsoft.JSInterop;

namespace ACore.Blazor.Modules.LocalizationModule;

// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Is registered in <see cref="AppManager.RegisterAllAppExtensions"/>
/// </summary>
public class LocalizationAppExtension(IAppManager appManager) : IAppExtension
{
  /// <summary>
  /// It will be called if the localization changes.
  /// </summary>
  public event Func<Task>? OnLocalizationChangeAsync;

  /// <summary>
  /// Get current language from storage.
  /// </summary>
  public async Task SetStartupLanguage(int defaultLanguage, IMediator mediatorInComponentContext)
  {
    await appManager.AppEnvironment.SetStartLanguage(defaultLanguage);
  }

  /// <summary>
  /// Notify app that language is changed.
  /// </summary>
  public async Task NotifyLanguageChangeAsync(int lcid, IMediator mediatorInComponentContext, IJSRuntime jsRuntime)
  {

    //appManager.

    //logger.LogInformation("Change culture {culture}", lcid);
    await mediatorInComponentContext.Send(new WriteAnalyticsCommand(new AnalyticsData
    {
      AnalyticsTypeEnum = AnalyticsTypeEnum.UI,
      Name = AnalyticsName.CultureChange,
      Value = lcid.ToString()
    }));

    await appManager.AppEnvironment.ChangeLanguage(lcid);


    OnLocalizationChangeAsync?.Invoke();
  }
}